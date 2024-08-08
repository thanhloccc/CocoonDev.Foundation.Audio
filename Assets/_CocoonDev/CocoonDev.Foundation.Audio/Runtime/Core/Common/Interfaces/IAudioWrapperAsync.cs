using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CocoonDev.Foundation.Audio
{
    public interface IAudioWrapperAsync
    {
        UniTask PlayAsync(AudioClip clip, float crossFadeTime);
        UniTask PauseAsync(float crossFadeTime);
        UniTask ResumeAsync(float crossFadeTime);
        UniTask StopAsync(float crossFadeTime);
    }
}
