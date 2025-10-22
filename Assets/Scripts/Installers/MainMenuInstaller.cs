using Zenject;
using Controllers;
using Models;
using Views;
using UnityEngine;
using Signals;

namespace Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [Header("Scene Views")]
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private SettingsView settingsView;

        public override void InstallBindings()
        {
            // Декларации сигналов
            Container.DeclareSignal<MainMenuSignals.PlayClicked>();
            Container.DeclareSignal<MainMenuSignals.SettingsClicked>();
            Container.DeclareSignal<SettingsSignals.MusicChanged>();
            Container.DeclareSignal<SettingsSignals.SfxChanged>();
            Container.DeclareSignal<SettingsSignals.BackClicked>();

            // Views из сцены
            Container.Bind<MainMenuView>().FromInstance(mainMenuView).AsSingle();
            Container.Bind<SettingsView>().FromInstance(settingsView).AsSingle();

            // Models
            Container.Bind<MainMenuModel>().AsSingle();
            Container.Bind<SettingsModel>().AsSingle();

            // Controllers (как InterfacesAndSelf → вызов Initialize())
            Container.BindInterfacesAndSelfTo<SettingsController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuController>().AsSingle().NonLazy();
        }
    }
}