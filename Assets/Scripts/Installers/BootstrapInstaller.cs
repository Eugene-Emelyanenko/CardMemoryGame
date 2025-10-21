using Zenject;
using UnityEngine;

namespace Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private string _mainMenuScene = "MainMenu";

        public override void InstallBindings()
        {
            Container.BindInstance(_mainMenuScene)
                .WhenInjectedInto<Bootstrap>();
            
            Container.BindInterfacesAndSelfTo<Bootstrap>()
                .AsSingle()
                .NonLazy();
        }
    }

}