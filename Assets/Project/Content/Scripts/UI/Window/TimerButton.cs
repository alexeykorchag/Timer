namespace Project.UI.Window
{
    using DG.Tweening;
    using System;
    using Cysharp.Threading.Tasks;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TimerButton : MonoBehaviour
    {
        [SerializeField]
        private string format;

        [SerializeField]
        private TMP_Text _textOnButton;

        [SerializeField]
        private Button _btn;

        public event Action<TimerData> Click;

        private TimerData _data;
        private ITimerButtonAnimation _buttonAnimation;

        public void Init(TimerData data)
        {
            _data = data;
            _buttonAnimation = new TimerButtonAnimation(_data.Index, this.GetComponent<RectTransform>(), this.GetComponent<CanvasGroup>());

            _textOnButton.text = string.Format(format, _data.Index);
        }

        public async UniTask Show()
        {
            _data.OnOver += OnOver;
            _btn.onClick.AddListener(OnClick);

            await _buttonAnimation.Show();
        }

        public async UniTask Hide()
        {
            _data.OnOver -= OnOver;
            _btn.onClick.RemoveListener(OnClick);

            await _buttonAnimation.Hide();
        }

        private void OnOver()
        {

        }

        private void OnClick()
        {
            Click?.Invoke(_data);
        }
    }


    public interface ITimerButtonAnimation
    {
        UniTask Show();
        UniTask Hide();
        void Over();
    }


    public class TimerButtonAnimation : ITimerButtonAnimation
    {
        private readonly int _index;
        private readonly RectTransform _rectTransform;
        private readonly CanvasGroup _canvasGroup;

        private static readonly float _outPosition = -Screen.width / 2f;
        private static readonly float _duration = 1;
        private static readonly float _delay = 0.1f;

        private Sequence _sequence;

        public TimerButtonAnimation(int index, RectTransform rectTransform, CanvasGroup canvasGroup)
        {
            _index = index;
            _rectTransform = rectTransform;
            _canvasGroup = canvasGroup;
        }

        public async UniTask Show()
        {
            var delay = _delay * _index;

            _canvasGroup.alpha = 0;
            _rectTransform.anchoredPosition = new Vector2(_outPosition, _rectTransform.anchoredPosition.y);

            _sequence?.Kill();

            _sequence = DOTween.Sequence()
            .Insert(delay, _rectTransform.DOAnchorPosX(0, _duration))
            .Insert(delay, _canvasGroup.DOFade(1, _duration));

            await _sequence.AsyncWaitForCompletion();
        }

        public async UniTask Hide()
        {
            _sequence?.Kill();

            _sequence = DOTween.Sequence()
            .Insert(0, _rectTransform.DOAnchorPosX(_outPosition, _duration))
            .Insert(0, _canvasGroup.DOFade(0, _duration));

            await _sequence.AsyncWaitForCompletion();
        }

        public void Over()
        {
            _sequence?.Kill();

            _sequence = DOTween.Sequence()
            .Insert(0, _rectTransform.DOShakeScale(_duration));
        }
    }
}