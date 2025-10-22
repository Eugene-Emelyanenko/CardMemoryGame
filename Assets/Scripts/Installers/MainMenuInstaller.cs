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
            BindMainMenu();
            
            BindSettings();
        }
        
        private void BindMainMenu()
        {
            Container.DeclareSignal<MainMenuSignals.PlayClicked>();
            Container.DeclareSignal<MainMenuSignals.SettingsClicked>();
            
            Container.Bind<MainMenuView>().FromInstance(mainMenuView).AsSingle();
            Container.Bind<MainMenuModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuController>().AsSingle().NonLazy();
        }

        private void BindSettings()
        {
            Container.DeclareSignal<SettingsSignals.MasterChanged>();
            Container.DeclareSignal<SettingsSignals.MusicChanged>();
            Container.DeclareSignal<SettingsSignals.SfxChanged>();
            Container.DeclareSignal<SettingsSignals.BackClicked>();

            Container.Bind<SettingsModel>().AsSingle();
            Container.Bind<SettingsView>().FromInstance(settingsView).AsSingle();
            Container.BindInterfacesAndSelfTo<SettingsController>().AsSingle().NonLazy();
        }
    }
}