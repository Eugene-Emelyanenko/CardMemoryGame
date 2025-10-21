using Controllers;
using DontDestroyOnLoad;
using Models;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private GameObject loadingScreenPrefab;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            BindGlobalCoroutineRunner();
            
            BindLoadingScreen();
        }

        private void BindGlobalCoroutineRunner()
        {
            Container.Bind<GlobalCoroutineRunner>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }

        private void BindLoadingScreen()
        {
            Container.Bind<LoadingScreenView>()
                .FromComponentInNewPrefab(loadingScreenPrefab)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<LoadingScreenModel>().AsSingle();
            Container.Bind<LoadingScreenController>().AsSingle().NonLazy();
        }
    }
}