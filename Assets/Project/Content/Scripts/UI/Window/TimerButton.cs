namespace Project.UI.Window
{
    using DG.Tweening;
    using UnityEngine;
    using TMPro;

    public class TimerButton : MonoBehaviour
    {
        [SerializeField]
        private string format;

        [SerializeField]
        private TMP_Text _textOnButton;

        private int _index;
        private ITimerButtonAnimation _buttonAnimation;

        public void Init(int index)
        {
            _index = index;
            _buttonAnimation = new TimerButtonAnimation(_index, this.GetComponent<RectTransform>(), this.GetComponent<CanvasGroup>());

            _textOnButton.text = string.Format(format, index);
        }

        public void Show()
        {
            _buttonAnimation.Show();
        }

        public void Hide()
        {
            _buttonAnimation.Hide();
        }
    }


    public interface ITimerButtonAnimation
    {
        void Show();
        void Hide();
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

        public void Show()
        {
            var delay = _delay * _index;

            _canvasGroup.alpha = 0;
            _rectTransform.anchoredPosition = new Vector2(_outPosition, _rectTransform.anchoredPosition.y);

            _sequence?.Kill();

            _sequence = DOTween.Sequence()
            .Insert(delay, _rectTransform.DOAnchorPosX(0, _duration))
            .Insert(delay, _canvasGroup.DOFade(1, _duration));
        }

        public void Hide()
        {
            _sequence?.Kill();

            _sequence = DOTween.Sequence()
            .Insert(0, _rectTransform.DOAnchorPosX(_outPosition, _duration))
            .Insert(0, _canvasGroup.DOFade(0, _duration));
        }
    }
}