namespace Project.UI.Window
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using Zenject;

    public class MainWindow : BaseWindow
    {
        [Inject]
        private UIController uiController;       
        [Inject]
        private TimeController _timeController;


        [Header("Timer")]
        [SerializeField]
        private int _btnOnStart = 3;
        [SerializeField]
        private RectTransform _btnTimerRoot;
        [SerializeField]
        private TimerButton _btnTimerPrefab;

        [Space]
        [SerializeField]
        private float _paddingTop;
        [SerializeField]
        private float _paddingBottom;
        [SerializeField]
        private float _spacing;

        private List<TimerButton> _timerBtns = new List<TimerButton>();

        public override void Init()
        {
            for (var i = 0; i < _btnOnStart; i++)            
                CreateTimerButton();            
        }

        public override async UniTask AfterShow()
        {
            var tasks = new UniTask[_timerBtns.Count];
            for (var i = 0; i < _timerBtns.Count; i++)
            {
                _timerBtns[i].Click += OnClickTimerButton;
                tasks[i] = _timerBtns[i].Show();
            }
            await UniTask.WhenAll(tasks);
        }

        public override async UniTask BeforeHide()
        {
            var tasks = new UniTask[_timerBtns.Count];
            for (var i = 0; i < _timerBtns.Count; i++)
            {
                _timerBtns[i].Click -= OnClickTimerButton;
                tasks[i] = _timerBtns[i].Hide();
            }
            await UniTask.WhenAll(tasks);
        }

        private TimerButton CreateTimerButton()
        {
            var btnTimer = Instantiate(_btnTimerPrefab, _btnTimerRoot);

            _timerBtns.Add(btnTimer);

            var data = _timeController.GetTimer(_timerBtns.Count);
            btnTimer.Init(data);

            UpdateContentSize();

            return btnTimer;
        }

        private async void OnClickTimerButton(TimerData timerData)
        {
           await uiController.OpenWindow<TimerWindow>(timerData);
        }

        private void UpdateContentSize()
        {
            var elemetH = _btnTimerPrefab.GetComponent<RectTransform>().sizeDelta.y;

            for (var i = 0; i < _timerBtns.Count; i++)
            {
                var y = _paddingTop /*+ elemetH / 2*/ + elemetH * i + _spacing * i;

                _timerBtns[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -y);
            }

            var contentH = _paddingTop + (elemetH * _timerBtns.Count) + (_spacing * _timerBtns.Count + 1) + _paddingBottom;

            _btnTimerRoot.sizeDelta = new Vector2(_btnTimerRoot.sizeDelta.x, contentH);
        }

    }
}
