using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace karin
{

    public static class FileSystem
    {
        public static string dataPath = Application.dataPath; // C:/NoGit/GameJam/Assets

        public static List<string> GetTextData(string path)
        {
            FileStream fs = new FileStream($"{dataPath}{path}", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line = sr.ReadToEnd();
            List<string> result = line.Split('\n').ToList();
            result.Reverse();
            return result;
        }

    }
}