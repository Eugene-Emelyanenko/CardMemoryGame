using Controllers;
using Models;
using Signals;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Scene Views")]
        [SerializeField] private PauseView pauseView;
        public override void InstallBindings()
        {
            BindPause();
        }

        private void BindPause()
        {
            Container.DeclareSignal<PauseSignals.PauseClicked>();
            Container.DeclareSignal<PauseSignals.ContinueClicked>();
            Container.DeclareSignal<PauseSignals.MenuClicked>();
            
            Container.Bind<PauseView>().FromInstance(pauseView).AsSingle();
            Container.Bind<PauseModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseController>().AsSingle().NonLazy();
        }
    }
}