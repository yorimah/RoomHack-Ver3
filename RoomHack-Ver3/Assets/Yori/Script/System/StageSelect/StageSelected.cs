using Cysharp.Threading.Tasks;
using UnityEngine;
public class StageSelected : MonoBehaviour
{
    /// <summary>
    /// あとで変更べつスクリプトで変更できるようにする
    /// </summary>
    [SerializeField, Header("ポップアップするオブジェクト")]
    private RectTransform windowObject;

    [SerializeField]
    private RectTransform backGlound;

    [SerializeField]
    private GameObject AcceptButtom;

    private bool isClick = false;

    public void ClickStageSelect()
    {
        if (!isClick)
        {
            _ = PopUpStageSelect();
            isClick = true;
        }
    }

    public async UniTask PopUpStageSelect()
    {
        Debug.Log("開く");
        windowObject.sizeDelta += new Vector2(0, 10);
        while (windowObject.rect.width < 1500)
        {
            windowObject.sizeDelta += new Vector2(200, 0);
            backGlound.sizeDelta = windowObject.sizeDelta;
            await UniTask.WaitForSeconds(0.01f);
        }

        while (windowObject.rect.height < 1000)
        {
            windowObject.sizeDelta += new Vector2(0, 100);
            backGlound.sizeDelta = windowObject.sizeDelta;
            await UniTask.WaitForSeconds(0.01f);
        }
        AcceptButtom.GetComponent<RectTransform>().sizeDelta = windowObject.sizeDelta;
        AcceptButtom.SetActive(true);
    }

    public void Exit()
    {
        if (isClick)
        {
            _ = FadeOutWindow();
           
        }
    }

    public async UniTask FadeOutWindow()
    {
        AcceptButtom.SetActive(false);
        while (windowObject.rect.height >= 0)
        {
            windowObject.sizeDelta -= new Vector2(0, 100);
            backGlound.sizeDelta = windowObject.sizeDelta;
            await UniTask.WaitForSeconds(0.01f);
        }
        windowObject.sizeDelta += new Vector2(0, 10);
        while (windowObject.rect.width >= 0)
        {
            windowObject.sizeDelta -= new Vector2(200, 0);
            backGlound.sizeDelta = windowObject.sizeDelta;
            await UniTask.WaitForSeconds(0.01f);
        }
        AcceptButtom.SetActive(false);
        windowObject.sizeDelta = Vector2.zero;
        backGlound.sizeDelta = windowObject.sizeDelta;
        isClick = false;
    }
}

