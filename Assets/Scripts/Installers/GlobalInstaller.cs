using Audio;
using Controllers;
using Global;
using Models;
using Signals;
using UnityEngine;
using UnityEngine.Audio;
using Views;
using Zenject;

namespace Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [Header("LoadingScreen")]
        [SerializeField] private GameObject loadingScreenPrefab;
                
        [Header("Audio")]
        [SerializeField] private AudioLibrary audioLibrary;
        [SerializeField] private AudioMixerRefs audioMixerRefs;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            BindGlobalCoroutineRunner();
            
            BindLoadingScreen();
            
            BindAudio();
            
            Container.Bind<SpriteStorage>().AsSingle();
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
            
            Container.BindInterfacesAndSelfTo<LoadingScreenController>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAudio()
        {
            Container.DeclareSignal<AudioSignals.PlayMusic>();
            Container.DeclareSignal<AudioSignals.StopMusic>();
            Container.DeclareSignal<AudioSignals.PlaySfx>();
            Container.DeclareSignal<AudioSignals.SetMasterVolume>();
            Container.DeclareSignal<AudioSignals.SetMusicVolume>();
            Container.DeclareSignal<AudioSignals.SetSfxVolume>();
            
            var audioRoot = new GameObject("[Audio]");
            audioRoot.transform.SetParent(ProjectContext.Instance.transform, false);
            
            Container.BindInstance(audioLibrary).AsSingle();
            Container.BindInstance(audioRoot.transform).AsSingle();
            
            Container.BindInstance(audioMixerRefs).AsSingle();

            Container.BindInterfacesAndSelfTo<AudioManager>()
                .AsSingle()
                .NonLazy();
        }
    }
}