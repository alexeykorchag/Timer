namespace Project.UI
{
    using Cysharp.Threading.Tasks;
    using Project.UI.Settings;
    using Project.UI.Window;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class UIController : MonoBehaviour
    {
        [Inject]
        private UIFactory _uiFactory;

        [SerializeField]
        private UISettings settings;

        [SerializeField]
        private Transform root;

        private readonly Dictionary<Type, BaseWindow> _dictWindowsByType = new Dictionary<Type, BaseWindow>();

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var window in settings.Windows)
            {
                if (window == null) continue;

                var windowType = window.GetType();

                if (_dictWindowsByType.ContainsKey(windowType))
                    throw new System.Exception($"UIController duplicate window:{windowType}");

                var newWindow = _uiFactory.Create(window.gameObject, root);

                _dictWindowsByType.Add(windowType, newWindow);

                newWindow.Init();
            }

            OpenWindow(_dictWindowsByType[settings.DefaultWindow.GetType()]);
        }

        public T GetWindow<T>() where T : BaseWindow
        {
            var windowType = typeof(T);
            if (_dictWindowsByType.TryGetValue(windowType, out var window))
                return window as T;
            return null;
        }

        public async UniTask<T> OpenWindow<T>(params object[] objs) where T : BaseWindow
        {
            var targetWindow = GetWindow<T>();
            if (targetWindow == null) throw new Exception($"UIController not found window:{typeof(T).ToString()}");

            return await OpenWindow(targetWindow, objs);
        }

        public async UniTask<T> OpenWindow<T>(T targetWindow, params object[] objs) where T : BaseWindow
        {
            foreach (var window in _dictWindowsByType.Values)
            {
                if (window == null) continue;
                if (!window.IsOpen) continue;
                if (window == targetWindow) continue;

                await window.Hide();
            }

            targetWindow.SetParams(objs);
            await targetWindow.Show();

            return targetWindow;
        }
    }
}