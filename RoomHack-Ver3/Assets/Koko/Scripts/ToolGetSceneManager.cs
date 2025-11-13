using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ToolGetSceneManager : MonoBehaviour
{
    [SerializeField, Header("ToolUIPrefabをアタッチ")]
    ToolUI toolUIPrefab;

    [SerializeField, Header("tooldataアタッチ")]
    ToolDataBank toolDataBank;

    [SerializeField, Header("textアタッチ")]
    GeneralUpdateText explainText;

    [SerializeField, Header("選択肢に出るツールをつっこむべし")]
    List<ToolTag> addToolList = new List<ToolTag>();

    List<ToolUI> toolUIList = new List<ToolUI>();

    [SerializeField]
    float sideSpace = 500;

    bool isSelected = false;

    private void Start()
    {
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
        SaveManager.Instance.Save(data);

        // シーン移動
        SceneManager.LoadScene("TitleDemoScene");
    }

}
