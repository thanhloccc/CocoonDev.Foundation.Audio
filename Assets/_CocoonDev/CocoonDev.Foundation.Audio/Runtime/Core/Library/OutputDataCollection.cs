using System;
using CocoonDev.Foundation.UniEnum;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace CocoonDev.Foundation.Audio
{
    [CreateAssetMenu(fileName = nameof(OutputDataCollection), menuName = "CocoonDev/Audio/Output Data Collection")]
    public class OutputDataCollection : ScriptableObject
    {
        [Title("Individual Output Data", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "identifier")]
        private OutputData[] _outputDatas;


        [Title("Configuration", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private string _enumName = "OutputID";
        [SerializeField, ReadOnly]
        private string _namespaceName = "CocoonDev.Foundation.Audio";
        [SerializeField, ReadOnly]
        private string _enumFilePath = "Assets/_CocoonDev/CocoonDev.Foundation.Audio/Runtime/Core/Common/";

        public OutputData[] OutputData
        {
            get => _outputDatas;
        }

#if UNITY_EDITOR
        [Button(buttonSize: 35), GUIColor("Yellow")]
        public void CodeGen()
        {
            var enumValues = new string[_outputDatas.Length];
            int index = 0;

            if (_outputDatas.IsNullOrEmpty())
            {
                Debug.LogWarning("[AudioEngine]: The OutputDatas array is either null or empty");
            }
            else
            {
                foreach (var sound in _outputDatas)
                {
                    enumValues[index] = sound.identifier;
                    index++;
                }
            }
            
            using (EnumExtensionsForGenerator sourceGen = new EnumExtensionsForGenerator())
            {
                sourceGen.AddEnumValuesToEnum(_enumName, enumValues, _enumFilePath, _namespaceName);
            }
        }
    }
#endif
}
