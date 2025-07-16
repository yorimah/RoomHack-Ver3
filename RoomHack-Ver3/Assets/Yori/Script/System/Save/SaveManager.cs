using System.IO;
using UnityEngine;

public class SaveManager 
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
    private string SaveFilePath => GetSavePath("player_save.json");

    public void Save(PlayerSaveData data)
    {
        string json = JsonUtility.ToJson(data, true); // trueで整形出力
        File.WriteAllText(SaveFilePath, json);
        Debug.Log("保存成功：" + SaveFilePath);
    }

    public PlayerSaveData Load()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.Log("セーブファイルが見つかりません。新規作成。");
            return new PlayerSaveData(); // 新規データ
        }

        string json = File.ReadAllText(SaveFilePath);
        return JsonUtility.FromJson<PlayerSaveData>(json);
    }

    public void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
            Debug.Log("セーブファイルを削除しました。");
        }
    }
}