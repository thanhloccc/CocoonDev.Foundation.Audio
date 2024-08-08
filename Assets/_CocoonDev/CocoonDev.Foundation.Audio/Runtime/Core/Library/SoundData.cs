using UnityEngine.Audio;
using UnityEngine;

namespace CocoonDev.Foundation.Audio
{
    [System.Serializable]
    public class SoundData
    {
        public string identifier;
        public bool loop;
        public bool frequentSound;
        public AudioClip[] clips;
        public AudioMixerGroup mixerGroup;

        public AudioClip GetClip()
        {
            return clips[Random.Range(0, clips.Length)];
        }
    }
}
