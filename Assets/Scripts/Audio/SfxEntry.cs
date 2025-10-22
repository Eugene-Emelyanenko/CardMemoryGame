using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class SfxEntry
    {
        public string id; 
        public AudioClip clip; 
        public float baseVolume = 1f;
    }
}