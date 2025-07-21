using System.IO;
using UnityEngine;

public static class SavePathProvider
{
    public static string GetSavePath(string fileName)
    {
#if UNITY_EDITOR
        string editorFolder = Path.Combine(Application.dataPath, "EditorSaves");
        if (!Directory.Exists(editorFolder))
        {
            Directory.CreateDirectory(editorFolder);
        }
        return Path.Combine(editorFolder, fileName);
#else
        string runtimeFolder = Application.persistentDataPath;
        if (!Directory.Exists(runtimeFolder))
        {
            Directory.CreateDirectory(runtimeFolder);
        }
        return Path.Combine(runtimeFolder, fileName);
#endif
    }
}
