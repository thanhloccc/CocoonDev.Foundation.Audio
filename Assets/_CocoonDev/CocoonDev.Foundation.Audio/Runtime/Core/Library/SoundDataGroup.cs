using System;
using UnityEngine.Audio;
using UnityEngine;

namespace CocoonDev.Foundation.Audio
{
    [System.Serializable]
    public class SoundDataGroup
    {
        public string identifier;
        public bool loop;
        public bool frequentSound;
        public AudioClip[] clips;
        public AudioMixerGroup mixerGroup;

        public ReadOnlyMemory<AudioClip> Clips
        {
            get => clips;
        }
    }
}
