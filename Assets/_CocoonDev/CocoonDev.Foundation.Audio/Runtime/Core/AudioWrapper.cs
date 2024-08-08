using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.Audio;
using PrimeTween;

using Random = UnityEngine.Random;

namespace CocoonDev.Foundation.Audio
{
    public class AudioWrapper : MonoBehaviour, IAudioWrapper, IAudioWrapperAsync
    {
        [SerializeField, Required]
        private AudioSource _source;

        [Title("Debug Info", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private bool _loop;
        [SerializeField, ReadOnly]
        private float _volume;

        private Tween _changeVolumeTween;
        private CancellationTokenSource _waitingCts;

        public LinkedListNode<AudioWrapper> Node { get; set; }

        public AudioWrapper WithLoop(bool loop)
        {
            _loop = loop;
            _source.loop = _loop;
            return this;
        }

        public AudioWrapper WithVolume(float volume)
        {
            _volume = volume;
            return this;
        }

        public AudioWrapper WithVolumePercentage(float percentage)
        {
            _source.volume *= percentage;
            return this;
        }

        public AudioWrapper WithPitch(float pitch)
        {
            _source.pitch = pitch;
            return this;
        }

        public AudioWrapper WithRandomPitch(float min = -0.05F, float max = 0.05F)
        {
            _source.pitch += Random.Range(min, max);
            return this;
        }

        public AudioWrapper WithSpatialSound(bool active)
        {
            _source.spatialBlend = active ? 1 : 0;
            return this;
        }

        public AudioWrapper WithHearDistance(Vector2 hearDistance)
        {
            _source.minDistance = hearDistance.x;
            _source.maxDistance = hearDistance.y;
            return this;
        }

        public AudioWrapper WithOutput(AudioMixerGroup audioMixer)
        {
            _source.outputAudioMixerGroup = audioMixer;
            return this;
        }

        public AudioWrapper WithPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }


        #region IAudioWrapper Implement
        public void ChangeVolune(float volune, float lerpTime = 0)
        {
            if (lerpTime > 0)
            {
                _changeVolumeTween = Tween.AudioVolume(_source, volune, lerpTime);
                return;
            }

            _source.volume = _volume;
        }

        public void Play(AudioClip clip, float crossFadeTime = 0)
        {
            PlayAndForget(clip, crossFadeTime).Forget();
        }

        public void Pause(float crossFadeTime = 0)
        {
            if (crossFadeTime > 0)
            {
                _changeVolumeTween = Tween.AudioVolume(_source, 0, crossFadeTime)
                    .OnComplete(OnPauseComplete);
                return;
            }

            OnPauseComplete();
        }

        public void Resume(float crossFadeTime = 0)
        {
            _source.UnPause();

            if (crossFadeTime > 0)
            {
                _source.volume = 0;
                _changeVolumeTween = Tween.AudioVolume(_source, _volume, crossFadeTime);
            }
        }

        public void Stop(float crossFadeTime = 0)
        {
            if (crossFadeTime > 0)
            {
                _changeVolumeTween = Tween.AudioVolume(_source, 0, crossFadeTime)
                    .OnComplete(OnStopComplete);
                return;
            }

            OnStopComplete();
        }
        #endregion

        #region IAudioWrapperAysnc Implement
        public async UniTaskVoid PlayAndForget(AudioClip clip, float crossFadeTime = 0)
        {
            await PlayAsync(clip, crossFadeTime);
        }

        public async UniTask PlayAsync(AudioClip clip, float crossFadeTime = 0)
        {
            if (clip == null)
            {
                Debug.LogError("[AudioEngine]: Audio clip is null");
                return;
            }

            _source.clip = clip;
            _source.Play();

            if (crossFadeTime > 0)
            {
                _source.volume = 0;
                _changeVolumeTween = Tween.AudioVolume(_source, _volume, crossFadeTime);
            }

            await WaitForSoundToEnd();
        }

        public UniTask PauseAsync(float crossFadeTime = 0)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ResumeAsync(float crossFadeTime = 0)
        {
            return UniTask.CompletedTask;
        }

        public UniTask StopAsync(float crossFadeTime = 0)
        {
            return UniTask.CompletedTask;
        }
        #endregion

        private async UniTask WaitForSoundToEnd()
        {
            RenewWaitingCts(ref _waitingCts);
            await UniTask.Delay(TimeSpan.FromSeconds(_source.clip.length)
                , cancellationToken: _waitingCts.Token);

            if (!_source.isPlaying)
            {
                Stop();
                return;
            }

            WaitForSoundToEnd().Forget();
        }

        private void RenewWaitingCts(ref CancellationTokenSource cts)
        {
            cts ??= new();
            if (cts.IsCancellationRequested)
            {
                cts.Dispose();
                cts = new();
            }

        }

        private void OnPauseComplete()
        {
            _source.Pause();
        }

        private void OnResumeComplete()
        {

        }

        private void OnStopComplete()
        {
            _waitingCts.Cancel();

            _source.Stop();
            AudioEngine.ReturnToPool(this);
        }

        
    }
}
