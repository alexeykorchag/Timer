namespace Project.Game
{
    using Project.UI;
    using UnityEngine;
    using Zenject;

    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private UIController uiController;

        [SerializeField]
        private TimeController timeController;

        public override void InstallBindings()
        {
            Container.BindInstance(uiController).AsSingle();
            Container.BindInstance(timeController).AsSingle();

            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();            
        }

    }
}