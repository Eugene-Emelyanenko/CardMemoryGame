using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Audio/Library", fileName = "AudioLibrary")]
    public class AudioLibrary : ScriptableObject
    {
        public MusicEntry[] music;
        public SfxEntry[] sfx;

        public AudioClip GetMusic(string id)
        {
            foreach (var e in music)
            {
                if (e.id == id)
                {
                    return e.clip;
                }
            }
            return null;
        }
        public (AudioClip clip, float baseVol) GetSfx(string id)
        {
            foreach (var e in sfx)
            {
                if (e.id == id)
                {
                    return (e.clip, Mathf.Clamp01(e.baseVolume));
                }
            }
            return (null, 1f);
        }
    }
}