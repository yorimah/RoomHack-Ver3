using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
// UnityEditor 名前空間はエディタ専用機能を使う場合に必要
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ToolGetSceneManager : MonoBehaviour
{
    [SerializeField, Header("ツールのデータ入力、外部から入力されたし")]
    ToolGetDataList toolGetDataList;

    [SerializeField, Header("選択肢の数")]
    int selectableToolNum = 3;

    [SerializeField, Header("ToolUIPrefabをアタッチ")]
    ToolUI toolUIPrefab;

    [SerializeField, Header("toolDataBankアタッチ")]
    ToolDataBank toolDataBank;

    [SerializeField, Header("textアタッチ")]
    GeneralUpdateText explainText;

    List<ToolTag> addToolList = new List<ToolTag>();

    List<ToolUI> toolUIList = new List<ToolUI>();

    [SerializeField]
    float sideSpace = 500;

    bool isSelected = false;

    // [HideInInspector] 実行時にはこの文字列だけあれば良いのでインスペクタからは隠す
    [HideInInspector]
    [SerializeField] private List<string> sceneToLoad;

    // #if UNITY_EDITOR ～ #endif で囲まれた部分はエディタ上でのみ有効になる
#if UNITY_EDITOR
    // インスペクタに表示するためのSceneAsset型変数
    [Header("遷移先シーン選択")] // インスペクタに見出しを表示
    [SerializeField] private List<SceneAsset> sceneAsset; // ここにシーンファイルをD&Dする
#endif

    private void Start()
    {
        // ランダムなtoolをリストに入力、重複しない
        List<ToolGetData> dataList = new List<ToolGetData>();
        dataList.AddRange(toolGetDataList.dataList);

        for (int i = 0; i < selectableToolNum; i++)
        {
            // 全体値を決定
            int totalWeight = 0;
            for (int j = 0; j < dataList.Count; j++)
            {
                totalWeight += dataList[j].toolWeight;
            }
            //Debug.Log(totalWeight);

            // ランダムの値を生成
            int rand = Random.Range(1, totalWeight);
            //Debug.Log(rand);

            // ランダムな値に該当するtoolを決定
            int nowWeight = 0;
            for (int j = 0; j < dataList.Count; j++)
            {
                nowWeight += dataList[j].toolWeight;
                if (nowWeight >= rand)
                {
                    // リストに追加
                    addToolList.Add(dataList[j].toolTag);
                    // 取得済みのツールをリストから削除
                    dataList.RemoveAt(j);
                    break;
                }
            }
            //Debug.Log(nowWeight);
        }


        // toolUI生成
        for (int i = 0; i < addToolList.Count; i++)
        {
            ToolUI instanceToolUI = Instantiate(toolUIPrefab, this.transform.position, Quaternion.identity, this.transform);
            toolUIList.Add(instanceToolUI);

            float offset = (-(sideSpace / 2) * (addToolList.Count - 1)) + (sideSpace * i);
            instanceToolUI.GetComponent<RectTransform>().localPosition = new Vector2(offset, -1000);
            instanceToolUI.toMovePosition = new Vector2(offset, 0);

            instanceToolUI.isOpen = true;
            instanceToolUI.thisTool = addToolList[i];
            instanceToolUI.toScale = new Vector2(1.5f, 1.5f);
        }
    }

    private void Update()
    {
        // 選択中
        if (!isSelected)
        {
            for (int i = 0; i < toolUIList.Count; i++)
            {
                // マウスがツールの上に載ったら
                if (toolUIList[i].isPointerOn)
                {
                    // 拡大して説明文
                    toolUIList[i].toScale = new Vector2(2, 2);
                    explainText.inputText = toolDataBank.toolDataList[(int)toolUIList[i].thisTool].toolText;

                    // 選択クリック
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        isSelected = true;
                        StartCoroutine(ToolSelected(toolUIList[i]));
                    }
                }
                else
                {
                    toolUIList[i].toScale = new Vector2(1.5f, 1.5f);
                }
            }
        }
    }

    // 選択後の演出
    IEnumerator ToolSelected(ToolUI _toolUI)
    {
        // 取らなかったやつ移動
        for (int i = 0; i < toolUIList.Count; i++)
        {
            float offset = (-(sideSpace / 2) * (addToolList.Count - 1)) + (sideSpace * i);
            toolUIList[i].toMovePosition = new Vector2(offset, 1000);
            toolUIList[i].isOpen = false;
        }
        _toolUI.isOpen = true;
        _toolUI.toMovePosition = Vector2.zero;

        yield return new WaitForSeconds(1f);

        _toolUI.toMovePosition = new Vector2(0, 1000);
        explainText.inputText = null;

        yield return new WaitForSeconds(1f);

        // データ追加
        PlayerSaveData data = SaveManager.Instance.Load();
        data.deckList.Add((int)_toolUI.thisTool);
        data.nowFloor++;
        if (data.nowFloor < data.stageRange)
        {
            int stageNum = Random.Range(0, sceneToLoad.Count - 1);
            // シーン移動
            if ((sceneToLoad[stageNum]) != null)
            {
                SaveManager.Instance.Save(data);
                SceneManager.LoadScene(sceneToLoad[stageNum]);
            }
            else
            {
                Debug.LogError("遷移先のシーン名が設定されていません！");
            }
        }
        else
        {
            SaveManager.Instance.Save(data);
            SceneManager.LoadScene("YoriTestScene");
        }
    }

    // OnValidateメソッドもエディタ専用
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
