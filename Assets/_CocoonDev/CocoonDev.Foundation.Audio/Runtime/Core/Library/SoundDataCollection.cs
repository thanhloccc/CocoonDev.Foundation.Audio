using CocoonDev.Foundation.UniEnum;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using UnityEngine;

namespace CocoonDev.Foundation.Audio
{
    [CreateAssetMenu(fileName = nameof(SoundDataCollection), menuName = "CocoonDev/Audio/Sound Data Collection")]
    public class SoundDataCollection : ScriptableObject
    {
        [Title("Individual Sound Data", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "identifier")]
        private SoundData[] _soundDatas;


        [Title("Configuration", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private string _enumName = "SoundID";
        [SerializeField, ReadOnly]
        private string _namespaceName = "CocoonDev.Foundation.Audio";
        [SerializeField, ReadOnly]
        private string _enumFilePath = "Assets/_CocoonDev/CocoonDev.Foundation.Audio/Runtime/Core/Common/";

        public ReadOnlyMemory<SoundData> SoundDatas
        {
            get => _soundDatas;
        }


        [Button(buttonSize: 35), GUIColor("Yellow")]
        public void CodeGen()
        {
            var enumValues = new string[_soundDatas.Length];
            int index = 0;

            if (_soundDatas.IsNullOrEmpty())
            {
                Debug.LogWarning("[AudioEngine]: The SoundDatas array is either null or empty");
            }
            else
            {
                foreach (var sound in _soundDatas)
                {
                    enumValues[index] = sound.identifier;
                    index++;
                }
            }

            //if (_soundDataGroups.IsNullOrEmpty())
            //{
            //    Debug.LogWarning("[AudioEngine]: The SoundDataGroups array is either null or empty");
            //}
            //else
            //{
            //    foreach (var soundGroup in _soundDataGroups)
            //    {
            //        enumValues[index] = soundGroup.identifier;
            //        index++;
            //    }
            //}

            using (EnumExtensionsForGenerator sourceGen = new EnumExtensionsForGenerator())
            {
                sourceGen.AddEnumValuesToEnum(_enumName, enumValues, _enumFilePath, _namespaceName);
            }
        }
    }
}
