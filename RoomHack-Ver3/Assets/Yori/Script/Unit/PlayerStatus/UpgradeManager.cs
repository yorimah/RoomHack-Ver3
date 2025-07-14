using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public enum UpgradeType
    {
        None,
        AttackUp,
        SpeedUp,
        HealthUp
    }
    private const string SaveKey = "StageUpgrades";

    // ステージごとの強化内容を保持する
    public Dictionary<string, UpgradeType> stageUpgrades = new();

    /// <summary>
    /// ステージの強化を設定
    /// </summary>
    public void SetUpgrade(string stageId, UpgradeType upgrade)
    {
        stageUpgrades[stageId] = upgrade;
    }

    /// <summary>
    /// ステージの強化を取得（なければNone）
    /// </summary>
    public UpgradeType GetUpgrade(string stageId)
    {
        return stageUpgrades.TryGetValue(stageId, out var result) ? result : UpgradeType.None;
    }

    /// <summary>
    /// 保存（PlayerPrefsに文字列として保存）
    /// </summary>
    public void Save()
    {
        // 例: "Stage1:1;Stage2:2" → (AttackUp = 1, SpeedUp = 2)
        List<string> entries = new();
        foreach (var pair in stageUpgrades)
        {
            entries.Add($"{pair.Key}:{(int)pair.Value}");
        }

        string saveStr = string.Join(";", entries);
        PlayerPrefs.SetString(SaveKey, saveStr);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 読み込み（PlayerPrefsから復元）
    /// </summary>
    public void Load()
    {
        stageUpgrades.Clear();

        string saved = PlayerPrefs.GetString(SaveKey, "");
        if (string.IsNullOrEmpty(saved)) return;

        string[] entries = saved.Split(';');
        foreach (string entry in entries)
        {
            string[] kv = entry.Split(':');
            if (kv.Length == 2)
            {
                string stageId = kv[0];
                if (int.TryParse(kv[1], out int upgradeVal))
                {
                    stageUpgrades[stageId] = (UpgradeType)upgradeVal;
                }
            }
        }
    }

    /// <summary>
    /// セーブデータを全消去（デバッグ用）
    /// </summary>
    public void ClearAll()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        stageUpgrades.Clear();
    }
}
