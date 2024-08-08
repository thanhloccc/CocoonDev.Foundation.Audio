using UnityEngine;

namespace CocoonDev.Foundation.Audio
{
    public interface IAudioWrapper
    {
        void ChangeVolune(float volune, float lerpTime);
        void Play(AudioClip clip, float crossFadeTime);
        void Pause(float crossFadeTime);
        void Resume(float crossFadeTime);
        void Stop(float crossFadeTime);
    }
}
