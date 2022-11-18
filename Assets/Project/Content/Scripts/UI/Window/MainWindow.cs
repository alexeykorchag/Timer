namespace Project.UI.Window
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MainWindow : BaseWindow
    {
        [SerializeField]
        private RectTransform _btnTimerRoot;
        [SerializeField]
        private TimerButton _btnTimerPrefab;

        [SerializeField]
        private float _paddingTop;
        [SerializeField]
        private float _paddingBottom;
        [SerializeField]
        private float _spacing;



        private List<TimerButton> _timerBtns = new List<TimerButton>();

        public override void Init()
        {
            base.Init();

            for (var i = 0; i < 3; i++)
            {
                CreateTimerButton();
            }

        }

        public override void Show()
        {
            base.Show();

            foreach (var timerBtn in _timerBtns)
                timerBtn.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        private TimerButton CreateTimerButton()
        {
            var btnTimer = Instantiate(_btnTimerPrefab, _btnTimerRoot);

            _timerBtns.Add(btnTimer);

            btnTimer.Init(_timerBtns.Count);

            UpdateContentSize();

            return btnTimer;
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
