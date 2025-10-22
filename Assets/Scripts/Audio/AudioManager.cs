// Audio/AudioManager.cs
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Global;
using Signals;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Audio
{
    public class AudioManager : IInitializable, System.IDisposable
    {
        private const string KEY_MASTER = "settings.master";
        private const string KEY_MUSIC  = "settings.music";
        private const string KEY_SFX    = "settings.sfx";

        private readonly SignalBus _bus;
        private readonly AudioLibrary _lib;
        private readonly Transform _root;
        private readonly GlobalCoroutineRunner _runner;

        private readonly AudioMixerGroup _masterGroup;
        private readonly AudioMixerGroup _musicGroup;
        private readonly AudioMixerGroup _sfxGroup;

        private AudioSource _musicA;
        private AudioSource _musicB;
        private bool _isAActive;
        private Tween _musicFadeTween;

        private readonly Queue<AudioSource> _sfxPool = new();
        private readonly List<AudioSource> _inUse = new();
        private const int InitialSfxPool = 8;
        
        private float _masterVol = 1f;
        private float _musicVol  = 1f;
        private float _sfxVol    = 1f;

        private const string MASTER_PARAM = "Master";
        private const string MUSIC_PARAM = "Music";
        private const string SFX_PARAM = "Sfx";

        public AudioManager(
            SignalBus bus,
            AudioLibrary lib,
            Transform audioRoot,
            GlobalCoroutineRunner runner,
            AudioMixerRefs audioMixerRefs)
        {
            _bus         = bus;
            _lib         = lib;
            _root        = audioRoot;
            _runner      = runner;
            _masterGroup = audioMixerRefs.Master;
            _musicGroup  = audioMixerRefs.Music;
            _sfxGroup    = audioMixerRefs.Sfx;
        }

        public void Initialize()
        {
            _masterVol = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
            _musicVol  = PlayerPrefs.GetFloat(KEY_MUSIC,  1f);
            _sfxVol    = PlayerPrefs.GetFloat(KEY_SFX,    1f);
            
            _musicA = CreateMusicSource("MusicA");
            _musicB = CreateMusicSource("MusicB");

            for (int i = 0; i < InitialSfxPool; i++)
            {
                _sfxPool.Enqueue(CreateSfxSource($"SFX_{i}"));
            }
            
            ApplyMixerVolume(_masterGroup.audioMixer, MASTER_PARAM, _masterVol);
            ApplyMixerVolume(_musicGroup.audioMixer, MUSIC_PARAM, _musicVol);
            ApplyMixerVolume(_sfxGroup.audioMixer, SFX_PARAM, _sfxVol);
            
            _bus.Subscribe<AudioSignals.PlayMusic>(OnPlayMusic);
            _bus.Subscribe<AudioSignals.StopMusic>(OnStopMusic);
            _bus.Subscribe<AudioSignals.PlaySfx>(OnPlaySfx);

            _bus.Subscribe<AudioSignals.SetMasterVolume>(s => SetMasterVolume(s.Value));
            _bus.Subscribe<AudioSignals.SetMusicVolume> (s => SetMusicVolume (s.Value));
            _bus.Subscribe<AudioSignals.SetSfxVolume>   (s => SetSfxVolume   (s.Value));
        }

        public void Dispose()
        {
            _bus.TryUnsubscribe<AudioSignals.PlayMusic>(OnPlayMusic);
            _bus.TryUnsubscribe<AudioSignals.StopMusic>(OnStopMusic);
            _bus.TryUnsubscribe<AudioSignals.PlaySfx>(OnPlaySfx);

            _musicFadeTween?.Kill();
        }
        
        private void OnPlayMusic(AudioSignals.PlayMusic s)
        {
            var clip = _lib.GetMusic(s.Id);
            if (!clip) return;

            var srcIn  = _isAActive ? _musicB : _musicA;
            var srcOut = _isAActive ? _musicA : _musicB;
            _isAActive = !_isAActive;

            srcIn.clip = clip;
            srcIn.loop = s.Loop;
            srcIn.volume = 0f;
            srcIn.Play();

            _musicFadeTween?.Kill();
            _musicFadeTween = DOTween.Sequence()
                .Join(srcIn.DOFade(1f, s.Fade).SetUpdate(true))
                .Join(srcOut.DOFade(0f, s.Fade).SetUpdate(true))
                .OnComplete(() => { if (srcOut.isPlaying) srcOut.Stop(); });
        }

        private void OnStopMusic(AudioSignals.StopMusic s)
        {
            var src = _isAActive ? _musicA : _musicB;
            _musicFadeTween?.Kill();
            _musicFadeTween = src.DOFade(0f, s.Fade).SetUpdate(true).OnComplete(() => src.Stop());
        }

        private AudioSource CreateMusicSource(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(_root, false);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = true;
            src.spatialBlend = 0f;
            src.outputAudioMixerGroup = _musicGroup;
            src.volume = 0f;
            return src;
        }

        private void OnPlaySfx(AudioSignals.PlaySfx s)
        {
            var (clip, baseVol) = _lib.GetSfx(s.Id);
            if (!clip) return;

            var src = PopSfx();
            src.spatialBlend = 0f;
            src.volume = Mathf.Clamp01(baseVol * s.Volume);
            src.PlayOneShot(clip);

            _runner.Run(ReturnWhenDone(src, clip.length));
        }

        private IEnumerator ReturnWhenDone(AudioSource src, float time)
        {
            _inUse.Add(src);
            yield return new WaitForSeconds(time);
            if (src != null)
            {
                src.Stop();
                _inUse.Remove(src);
                _sfxPool.Enqueue(src);
            }
        }

        private AudioSource PopSfx()
        {
            if (_sfxPool.Count == 0)
                _sfxPool.Enqueue(CreateSfxSource($"SFX_{_inUse.Count}"));
            return _sfxPool.Dequeue();
        }

        private AudioSource CreateSfxSource(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(_root, false);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.outputAudioMixerGroup = _sfxGroup;
            return src;
        }
        
        private void SetMasterVolume(float v)
        {
            _masterVol = Mathf.Clamp01(v);
            ApplyMixerVolume(_masterGroup.audioMixer, MASTER_PARAM, _masterVol);
        }

        private void SetMusicVolume(float v)
        {
            _musicVol = Mathf.Clamp01(v);
            ApplyMixerVolume(_musicGroup.audioMixer, MUSIC_PARAM, _musicVol);
        }

        private void SetSfxVolume(float v)
        {
            _sfxVol = Mathf.Clamp01(v);
            ApplyMixerVolume(_sfxGroup.audioMixer, SFX_PARAM, _sfxVol);
        }
        
        private static void ApplyMixerVolume(AudioMixer mixer, string exposedParam, float linear)
        {
            float dB = (linear <= 0.0001f) ? -80f : Mathf.Log10(linear) * 20f;
            mixer.SetFloat(exposedParam, dB);
        }
    }
}
