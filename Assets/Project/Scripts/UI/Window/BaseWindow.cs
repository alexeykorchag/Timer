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

        public UniTask Show()
        {
            BeforeShow();

            IsOpen = true;
            gameObject.SetActive(true);

            AfterShow();

            return UniTask.CompletedTask;
        }
        public virtual UniTask BeforeShow()
        {
            return UniTask.CompletedTask;
        }
        public virtual UniTask AfterShow()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Hide()
        {
            BeforeHide();

            IsOpen = false;
            this.gameObject.SetActive(false);

            AfterHide();

            return UniTask.CompletedTask;
        }
        public virtual UniTask BeforeHide()
        {
            return UniTask.CompletedTask;
        }
        public virtual UniTask AfterHide()
        {
            return UniTask.CompletedTask;
        }

        public virtual void SetParams(params object[] objs) { }
    }
}