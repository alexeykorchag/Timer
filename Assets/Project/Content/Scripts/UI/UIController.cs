namespace Project.UI
{
    using Project.UI.Settings;
    using Project.UI.Window;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Cysharp.Threading.Tasks;

    public class UIController : MonoBehaviour
    {

        private static UIController _instance;

        public static UIController Inctance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<UIController>();
                return _instance;
            }
        }


        [SerializeField]
        private UISettings settings;

        [SerializeField]
        private Transform root;

        private readonly Dictionary<int, BaseWindow> _dictWindowsByType = new Dictionary<int, BaseWindow>();

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
                var hashCode = windowType.GetHashCode();

                if (_dictWindowsByType.ContainsKey(hashCode))
                    throw new System.Exception($"UIController duplicate window:{windowType}");

                var newWindowObject = Instantiate(window.gameObject, root);
                var newWindow = newWindowObject.GetComponent<BaseWindow>();

                _dictWindowsByType.Add(hashCode, newWindow);

                newWindow.Init();
            }

            OpenWindow(_dictWindowsByType[settings.DefaultWindow.GetType().GetHashCode()]);
        }

        public T GetWindow<T>() where T : BaseWindow
        {
            var hashCode = typeof(T).GetHashCode();
            if (_dictWindowsByType.TryGetValue(hashCode, out var window))
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