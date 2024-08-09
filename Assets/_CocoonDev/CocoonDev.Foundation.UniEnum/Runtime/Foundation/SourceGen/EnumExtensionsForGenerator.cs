using System;
using System.IO;
using UnityEditor;

namespace CocoonDev.Foundation.UniEnum
{
#if UNITY_EDITOR
    public class EnumExtensionsForGenerator : IDisposable
    {
        public void AddEnumValuesToEnum(string enumName
                , string[] valueNames
                , string enumFilePath
                , string namespaceName)
        {
            string ident = "	";
            string filePathAndName = enumFilePath + enumName + ".cs";
            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine($"namespace {namespaceName}");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine(ident + "public enum " + enumName);
                streamWriter.WriteLine(ident + "{");
                for (int i = 0; i < valueNames.Length; i++)
                {
                    streamWriter.WriteLine(ident + ident + valueNames[i] + (i == valueNames.Length - 1 ? "" : ","));
                }
                streamWriter.WriteLine(ident + "}");
                streamWriter.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

        public void Dispose()
        {

        }
#endif
    }
}
