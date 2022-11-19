namespace Project.UI.Window
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using System;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class TimerWindow : BaseWindow
    {
        [Inject]
        private UIController uiController;

        [SerializeField]
        private TMP_Text _textTime;

        [SerializeField]
        private RectTransform _panel;
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private AnimationCurve _curve;

        [SerializeField]
        private ButtonPressExtension _buttonDecrease;
        [SerializeField]
        private ButtonPressExtension _buttonIncrease;
        [SerializeField]
        private Button _buttonStart;
        [SerializeField]
        private Button _buttonClose;

        private TimerData _data;
        private ITimerWindowAnimation _animation;
     

        public override void Init()
        {
            base.Init();
            _animation = new TimerWindowAnimation(_panel, _canvasGroup);
        }

        public override void SetParams(params object[] objs)
        {
            _data = objs[0] as TimerData;
        }

        public override async UniTask AfterShow()
        {
            _buttonClose.onClick.AddListener(ClickClose);
            
            _buttonStart.onClick.AddListener(ClickStart);

            _buttonDecrease.onDown += OnDownDecrease;
            _buttonDecrease.onPressed += OnDownDecrease;

            _buttonIncrease.onDown += OnDownIncrease;
            _buttonIncrease.onPressed += OnDownIncrease;      
            
            _data.OnTimeSpanChanged += UpdateVisual;

            UpdateVisual();

            if (_data.IsStarted)
            {
                StartCoroutine(UpdateTimer());
            }

            await _animation.Show();
        }

        public override async UniTask BeforeHide()
        {
            _buttonClose.onClick.RemoveListener(ClickClose);

            _buttonStart.onClick.RemoveListener(ClickStart);

            _buttonDecrease.onDown -= OnDownDecrease;
            _buttonDecrease.onPressed -= OnDownDecrease;

            _buttonIncrease.onDown -= OnDownIncrease;
            _buttonIncrease.onPressed -= OnDownIncrease;

            _data.OnTimeSpanChanged -= UpdateVisual;

            await _animation.Hide();
        }

        private void OnDownDecrease()
        {
            _data.Decrease(GetTimeSpanFromCurve(_buttonDecrease));            
        }

        private void OnDownIncrease()
        {
            _data.Increase(GetTimeSpanFromCurve(_buttonIncrease));
        }

        private TimeSpan GetTimeSpanFromCurve(ButtonPressExtension btn) => TimeSpan.FromSeconds(_curve.Evaluate(btn.PressTime));

        private void ClickStart()
        {
            _data.Start();

            ClickClose();
        }

        private void ClickClose()
        {
            uiController.OpenWindow<MainWindow>();
        }

        private void UpdateVisual()
        {
            var timeSpan = _data.GetRemainingTime();

            _textTime.text = string.Format("{0} : {1:D2} : {2:D2}", (long)Math.Floor(timeSpan.TotalHours), timeSpan.Minutes, timeSpan.Seconds);

            _buttonDecrease.interactable = !_data.IsStarted && timeSpan > TimeSpan.Zero;
            _buttonIncrease.interactable = !_data.IsStarted;
            _buttonStart.interactable = !_data.IsStarted && timeSpan > TimeSpan.Zero;
        }

        private IEnumerator UpdateTimer()
        {
            var wfs = new WaitForSecondsRealtime(1f);

            while (!_data.IsFinished)
            {
                UpdateVisual();
                yield return wfs;
            }
        }

    }

    public interface ITimerWindowAnimation
    {
        UniTask Show();
        UniTask Hide();
    }

    public class TimerWindowAnimation : ITimerWindowAnimation
    {
        private readonly RectTransform _rectTransform;
        private readonly CanvasGroup _canvasGroup;

        //TODO: take out parameters for non-code configuration
        private static readonly float _outPosition = Screen.width / 2f;
        private static readonly float _duration = 1;

        private Sequence _sequence;

        public TimerWindowAnimation(RectTransform rectTransform, CanvasGroup canvasGroup)
        {
            _rectTransform = rectTransform;
            _canvasGroup = canvasGroup;
        }

        public async UniTask Show()
        {
            _canvasGroup.alpha = 0;
            _rectTransform.anchoredPosition = new Vector2(_outPosition, _rectTransform.anchoredPosition.y);

            _sequence?.Kill();

            _sequence = DOTween.Sequence()
            .Insert(0, _rectTransform.DOAnchorPosX(0, _duration))
            .Insert(0, _canvasGroup.DOFade(1, _duration));

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
    }

}
