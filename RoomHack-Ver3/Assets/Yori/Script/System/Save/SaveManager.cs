using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    private static SaveManager _instance;
    public static SaveManager Instance => _instance ??= new SaveManager();

    private string SaveFilePath => SavePathProvider.GetSavePath("player_save.json");

    /// <summary>
    /// dataをセーブする
    /// </summary>
    /// <param name="data"></param>
    public void Save(PlayerSaveData data)
    {
        if (data == null)
        {
            Debug.LogError("保存しようとしているデータが null です！");
            return;
        }

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log("保存完了：" + SaveFilePath + "\n内容:\n" + json);
    }

    /// <summary>
    /// セーブデータをロードする
    /// なかったら新規データ作成。
    /// </summary>
    /// <returns></returns>
    public PlayerSaveData Load()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.Log("新規データを作成します。");
            PlayerSaveData newsave = InitializeSaveData();
            Save(newsave);
            return newsave;
        }
        Debug.Log("ロードちゅー");
        var json = File.ReadAllText(SaveFilePath);
        return JsonConvert.DeserializeObject<PlayerSaveData>(json);

    }

    /// <summary>
    /// セーブデータ削除
    /// </summary>
    public void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
            Debug.Log("セーブデータを削除しました。");
        }
    }

    /// <summary>
    /// セーブデータ初期化
    /// </summary>
    /// <returns></returns>
    private PlayerSaveData InitializeSaveData()
    {
        return new PlayerSaveData
        {
            score_Stage = 0,
            score_DestoryEnemy = 0,
            gunName = GunName.HandGun,
            maxHitPoint = 100,
            moveSpeed = 5,
            maxRamCapacity = 10,
            ramRecovery = 0.01f, //実質廃止
            maxHandSize = 5,
            deckList = new List<int>
            {
                1,1,1,1,2,2,3,3,4
            }
        };
    }
}
