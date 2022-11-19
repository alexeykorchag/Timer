namespace Project.UI.Window
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public abstract class BaseWindow : MonoBehaviour
    {
        public bool IsOpen { get; protected set; }

        private void OnDestroy()
        {
            Destroy();
        }

        public virtual void Init() { }

        public virtual void Destroy() { }


        public virtual UniTask Show()
        {
            IsOpen = true;
            gameObject.SetActive(true);

            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide()
        {
            IsOpen = false;
            this.gameObject.SetActive(false);

            return UniTask.CompletedTask;
        }

        public virtual void SetParams(params object[] objs) { }
    }
}