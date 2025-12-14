using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

public class StageSelecter : MonoBehaviour
{
    // [HideInInspector] 実行時にはこの文字列だけあれば良いのでインスペクタからは隠す
    [HideInInspector]
    [SerializeField] private List<string> sceneToLoad;

    // #if UNITY_EDITOR ～ #endif で囲まれた部分はエディタ上でのみ有効になる
#if UNITY_EDITOR
    // インスペクタに表示するためのSceneAsset型変数
    [Header("遷移先シーン選択")] // インスペクタに見出しを表示
    [SerializeField] private List<SceneAsset> sceneAsset; // ここにシーンファイルをD&Dする
#endif
    [SerializeField, Header("ステージセレクトボタン")]
    private List<WindowStageSelect> selectButtomList = new List<WindowStageSelect>();

    void Start()
    {
        foreach (var selcetButtom in selectButtomList)
        {
            int rand = Random.Range(0, sceneToLoad.Count - 1);
            selcetButtom.SetScene(sceneToLoad[rand], 3);
            sceneToLoad.Remove(sceneToLoad[rand]);
        }
    }


#if UNITY_EDITOR
    // インスペクタで値が変更された時などに自動で呼ばれるメソッド
    private void OnValidate()
    {
        sceneToLoad.Clear();
        foreach (var scene in sceneAsset)
        {
            if (scene != null)
            {
                sceneToLoad.Add(scene.name); // 名前だけを保持
            }
        }
    }
#endif
}
