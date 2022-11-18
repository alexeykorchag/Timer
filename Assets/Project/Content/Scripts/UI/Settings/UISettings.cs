namespace Project.UI.Settings
{
    using Project.UI.Window;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UISettings", menuName = "Settings/UI/UISettings")]
    public class UISettings : ScriptableObject
    {
        [SerializeField]
        private BaseWindow defaultWindow;

        [SerializeField]
        private BaseWindow[] windows;

        public BaseWindow DefaultWindow => defaultWindow;
        public BaseWindow[] Windows => windows;
    }
}

