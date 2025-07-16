#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class EditorExitCleaner
{
    static EditorExitCleaner()
    {
        // エディタ終了時に呼ばれるイベントを登録
        EditorApplication.quitting += OnEditorQuit;
    }

    private static void OnEditorQuit()
    {
        string path = Path.Combine(Application.dataPath, "EditorSaves");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("[EditorExitCleaner] EditorSaves フォルダを削除しました。");
        }
    }
}
#endif
