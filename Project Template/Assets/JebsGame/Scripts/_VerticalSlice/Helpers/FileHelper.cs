using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JebsReadingGame.Helpers
{
    public static class FileHelpers
    {
        public static void ReplaceJson<T>(string fileName, T item)
        {
            WriteJson<T>(fileName, item, FileMode.Create, "", "");
        }

        public static void AppendJson<T>(string fileName, T item, string delimiterBefore)
        {
            WriteJson<T>(fileName, item, FileMode.Append, delimiterBefore, "");
        }

        public static T ReadJson<T>(string fileName)
        {
            string path = Application.persistentDataPath + "/" + fileName;

            string str = "";

            StreamReader reader = new StreamReader(path);
            str = reader.ReadToEnd();
            reader.Close();

            T item = JsonUtility.FromJson<T>(str);
            return item;
        }

        static void WriteJson<T>(string fileName, T item, FileMode fileMode, string before, string after)
        {
            string path = Application.persistentDataPath + "/" + fileName;

            string str = JsonUtility.ToJson(item, true);
            using (FileStream fs = new FileStream(path, fileMode))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(before);
                    writer.Write(str);
                    writer.Write(after);
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

        }
    }
}
