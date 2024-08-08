using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CocoonDev.Foundation.Audio
{
    [System.Serializable]
    public sealed class AudioToolsLibrary
    {
        [Title("Loader", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, Required]
        private SoundDataCollection _soundDataCollection;

        [Title("Debug Info", titleAlignment: TitleAlignments.Centered)]
        [ShowInInspector, ReadOnly]
        public Dictionary<SoundID, SoundData> CacheSoundDataById { get; private set; }

        [ShowInInspector, ReadOnly]
        public Dictionary<MusicID, SoundData> CacheMusicDataById { get; private set; }

        public void Initialize()
        {
            var soundIdValues = UniEnum.UniEnum.GetValues<SoundID>();
            CacheSoundDataById = new();

            for (int i = 0; i < soundIdValues.Count; i++)
            {
                CacheSoundDataById.Add(soundIdValues[i], _soundDataCollection.SoundDatas.Span[i]);
            }
        }

        public void Deinitialize()
        {

        }
    }
}
