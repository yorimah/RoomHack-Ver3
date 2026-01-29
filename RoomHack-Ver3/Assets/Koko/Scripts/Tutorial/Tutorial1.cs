using UnityEngine;
using System.Collections.Generic;

public class Tutorial1 : MonoBehaviour
{
    [SerializeField, Header("あたっち")]
    TutorialManager tutorialManager;

    [SerializeField]
    List<TutorialData> tutorialList = new List<TutorialData>();

    int index = 0;
    bool isExplain = false;

    float timer = 0;

    TutorialData nullData;

    private void Awake()
    {
        nullData.muskPos = Vector2.zero;
        nullData.muskSize = new Vector2(1920, 1080);
        nullData.textPos = Vector2.zero;
        nullData.explains = " ";
    }

    private void Update()
    {
        // 説明オンオフ
        if (isExplain)
        {
            tutorialManager.SetStatus(tutorialList[index]);
        }
        else
        {
            tutorialManager.SetStatus(nullData);
        }

        switch (index)
        {
            case 0:
                // 説明開始条件
                if (true) isExplain = true;

                // 説明中
                if (isExplain)
                {

                }

                // 説明終了条件
                if (true)
                {
                    isExplain = false;
                    index++;
                }

                break;
        }
    }
}

public class TutorialData
{
    public Vector2 muskPos;
    public Vector2 muskSize;

    public Vector2 textPos;
    public string explains;

    public float time;
}
