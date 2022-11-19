namespace Project.UI
{
    using Project.UI.Window;
    using UnityEngine;
    using Zenject;

    public class UIFactory
    {
        [Inject]
        private DiContainer container;

        public BaseWindow Create(GameObject prefab, Transform parent)
        {
            var newWindowObject = Object.Instantiate(prefab, parent);
            var newWindow = newWindowObject.GetComponent<BaseWindow>();

            container.Inject(newWindow);

            return newWindow;
        }
    }
}