using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField, Header("あたっち")]
    RectTransform muskRect;
    [SerializeField]Vector2 muskPos;
    [SerializeField]Vector2 muskSize;

    [SerializeField, Header("あたっち")]
    GeneralUpdateText explainText;
    [SerializeField] Vector2 textPos;
    [SerializeField] string explains;

    private void Update()
    {
        Vector2 moveVec = (muskPos - (Vector2)muskRect.localPosition) / 10;
        muskRect.localPosition += (Vector3)moveVec;

        Vector2 scaleVec = (muskSize - muskRect.sizeDelta) / 10;
        muskRect.sizeDelta += scaleVec;

        explainText.GetComponent<RectTransform>().localPosition = textPos;
        explainText.inputText = explains;
    }

    public void SetStatus(TutorialData _data)
    {
        muskPos = _data.muskPos;
        muskSize = _data.muskSize;
        textPos = _data.textPos;
        explains = _data.explains;
    }
}
