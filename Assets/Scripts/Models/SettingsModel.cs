using Base;
using UnityEngine;

namespace Models
{
    public class SettingsModel : Model
    {
        private const string MUSIC_KEY = "settings.music";
        private const string SFX_KEY   = "settings.sfx";
        private const string MASTER_KEY = "settings.master";

        public float MusicVolume { get; private set; } = 1f;
        public float SfxVolume { get; private set; } = 1f;
        public float MasterVolume { get; private set; } = 1f;

        public override void Initialize()
        {
            MusicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            SfxVolume = PlayerPrefs.GetFloat(SFX_KEY,   1f);
            MasterVolume = PlayerPrefs.GetFloat(MASTER_KEY,   1f);
        }

        public void SetMusic(float v) => MusicVolume = Mathf.Clamp01(v);
        public void SetSfx(float v) => SfxVolume = Mathf.Clamp01(v);
        public void SetMaster(float v) => MasterVolume = Mathf.Clamp01(v);

        public void Save()
        {
            PlayerPrefs.SetFloat(MUSIC_KEY, MusicVolume);
            PlayerPrefs.SetFloat(SFX_KEY, SfxVolume);
            PlayerPrefs.SetFloat(MASTER_KEY, MasterVolume);
            PlayerPrefs.Save();
        }
    }
}