using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [System.Serializable]
    public class AudioMixerRefs
    {
        [field: SerializeField] public AudioMixerGroup Master { get; private set; }
        [field: SerializeField] public AudioMixerGroup Music { get; private set; }
        [field: SerializeField] public AudioMixerGroup Sfx { get; private set; }
    }
}