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
           // exeがあるフォルダ
        string exeFolder = Path.GetDirectoryName(Application.dataPath);

        string saveFolder = Path.Combine(exeFolder, "Saves");
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        return Path.Combine(saveFolder, fileName);
#endif
    }
}
