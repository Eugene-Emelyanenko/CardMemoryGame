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

            // Global coroutine runner
            Container.Bind<GlobalCoroutineRunner>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            // Loading Screen (одна копия)
            Container.Bind<LoadingScreenView>()
                .FromComponentInNewPrefab(loadingScreenPrefab)
                .AsSingle()
                .NonLazy();

            Container.Bind<LoadingScreenModel>().AsSingle();

            // Важно: как InterfacesAndSelf, чтобы вызывался Initialize/Dispose
            Container.BindInterfacesAndSelfTo<LoadingScreenController>()
                .AsSingle()
                .NonLazy();
        }
    }
}