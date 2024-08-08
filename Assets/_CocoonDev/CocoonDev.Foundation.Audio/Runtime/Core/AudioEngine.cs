using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using ZBase.Foundation.Pooling.UnityPools;

namespace CocoonDev.Foundation.Audio
{
    public class AudioEngine : MonoBehaviour
    {
        private static AudioEngine s_instance;

        [Title("Libraries", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private AudioToolsLibrary _audioToolsLibrary;
        [SerializeField]
        private AudioMixerGroup _audioMixerGroupDefault;

        [Title("Pooler", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private bool _prepoolOnStart;
        [SerializeField]
        private int _prepoolAmount;
        [SerializeField]
        private AudioWrapper _sourcePool;

        private bool _initilized;

        private static ComponentPool<AudioWrapper, ComponentPrefab<AudioWrapper>> s_pool;
        private static readonly LinkedList<AudioWrapper> s_frequentSoundEmitters = new();

        private void Awake()
        {
            if (_initilized)
                return;
            s_instance = this;

            OnInitialize();
            InitializePool();
            OnAwake();

            _initilized = true;
        }

        private async void Start()
        {
            if (_prepoolOnStart)
                await s_pool.Prepool(this.GetCancellationTokenOnDestroy());

            await OnStart();
        }


        protected virtual void OnAwake()
        {

        }

        protected virtual async UniTask OnStart()
        {
            await UniTask.Delay(0);
        }

        private void OnInitialize()
        {
            _audioToolsLibrary.Initialize();
        }
        private void InitializePool()
        {
            s_pool = new(new ComponentPrefab<AudioWrapper> {
                Parent = transform,
                PrepoolAmount = _prepoolAmount,
                Source = _sourcePool,
            });
        }

        public static void PlaySound(SoundID soundID, float volumePercentage)
        {
            PlaySoundTask(soundID, volumePercentage).Forget();
        }

        public static async UniTaskVoid PlaySoundTask(SoundID soundID, float volumePercentage)
        {
            await PlaySoundAsync(soundID, volumePercentage);
        }

        public static async UniTask PlaySoundAsync(SoundID soundID, float volumePercentage)
        {
            if (TryGetSoundDataById(soundID, out var data))
            {
                var instance = await Get();

                s_instance.ConfigureDefaultAudioWrapperSettings(instance);
                await instance.WithVolumePercentage(volumePercentage)
                    .WithLoop(data.loop)
                    .WithOutput(GetAudioMixerGroup(data.outputID))
                    .PlayAsync(data.GetClip());
            }
        }

        public static AudioMixerGroup GetAudioMixerGroup(OutputID outputID)
        {
            if (s_instance._audioToolsLibrary.CacheOutputDataById.TryGetValue(outputID, out var data))
            {
                return data.mixerGroup;
            }

            return s_instance._audioMixerGroupDefault;
        }


        public static bool TryGetAudioMixerGroup(OutputID outputID, AudioMixerGroup mixerGroup)
        {
            if(s_instance._audioToolsLibrary.CacheOutputDataById.TryGetValue(outputID, out var data))
            {
                mixerGroup = data.mixerGroup;
                return true;
            }

            mixerGroup = s_instance._audioMixerGroupDefault;
            return false;
        }

        public static bool TryGetSoundDataById(SoundID id, out SoundData soundData)
        {
            if (s_instance._audioToolsLibrary.CacheSoundDataById.TryGetValue(id, out soundData))
            {
                return true;
            }

            Debug.LogError($"SoundData with SoundId '{id}' not found in the cache.");
            return false;

        }


        public static async UniTask<AudioWrapper> Get()
        {
            var instance = await s_pool.Rent();
            instance.gameObject.SetActive(true);
            return instance;
        }

        public static void ReturnToPool(AudioWrapper instance)
        {
            if (instance.Node != null)
            {
                s_frequentSoundEmitters.Remove(instance.Node);
                instance.Node = null;
            }

            s_pool.Return(instance);
        }

        private void ConfigureDefaultAudioWrapperSettings(AudioWrapper instance)
        {
            float volume = 1;

            instance.WithVolume(volume)
                .WithPitch(1.0F)
                .WithSpatialSound(false) // 2D Sound
                .WithOutput(null);
        }
    }
}
