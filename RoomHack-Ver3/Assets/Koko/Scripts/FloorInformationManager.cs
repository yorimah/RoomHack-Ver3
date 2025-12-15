using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections;

public class FloorInformationManager : MonoBehaviour
{
    [Inject]
    IGetCleaFlag floorData;

    [Inject]
    IGetSaveData getSaveData;

    PlayerSaveData saveData;

    [SerializeField] GeneralUpdateText floorText;
    [SerializeField] GeneralUpdateText readyText;

    [SerializeField] Text timerText;

    [SerializeField] Text effectTimerText;
    float effectTimerNum = 0;

    [SerializeField] Image startScreen;
    [SerializeField] Image clearScreen;
    //[SerializeField] Image redScreen;
    [SerializeField] Image bandScreen;

    float startTransparency = 0;
    float clearTransparency = 0;
    //float redScreenPosY = 0;


    bool isClearTrigger = false;

    private void Start()
    {
        EffectManager.Instance.ActEffect(EffectManager.EffectType.Time, new Vector2(0, 6));

        saveData = getSaveData.GetPlayerSaveData();

        StartCoroutine(StartSequence());

        startTransparency = 1;

        Debug.Log("SE_FloorReady");
    }

    private void Update()
    {
        if (effectTimerNum < 10)
        { 
            effectTimerNum += Time.deltaTime * 10 / 3;
        }
        else
        {
            effectTimerNum = 10;
        }
        effectTimerText.text = effectTimerNum.ToString("00.00");

        startScreen.color = new Color(0.066f, 0.055f, 0.09f, startTransparency);
        clearScreen.color = new Color(1, 1, 1, clearTransparency);
        startTransparency *= 0.98f;
        clearTransparency *= 0.95f;

        //redScreen.rectTransform.anchoredPosition = new Vector2(0, redScreenPosY);
        //redScreenPosY = timerText.GetComponent<StageTimerDemo>().

        if (floorData.isClear)
        {
            Debug.Log("SE_FloorClear");

            bandScreen.gameObject.SetActive(true);
            floorText.gameObject.SetActive(true);
            floorText.inputText = "FLOOR CLEAR";

            effectTimerText.gameObject.SetActive(true);
            effectTimerText.text = timerText.text;

            //this.transform.position = new Vector2(0, -20);

            if (!isClearTrigger)
            {
                clearTransparency = 0.25f;
                isClearTrigger = true;
            }
        }
    }

    IEnumerator StartSequence()
    {
        bandScreen.gameObject.SetActive(true);

        timerText.gameObject.SetActive(false);
        floorText.gameObject.SetActive(true);
        floorText.inputText = "FLOOR " + saveData.nowFloor;
        readyText.GetComponent<Text>().color = new Color32(255, 86, 81, 192);
        readyText.inputText = "loading...";
        //this.transform.position = new Vector2(0, 20);

        GameTimer.Instance.playTime = 0;

        yield return new WaitForSeconds(3f);

        readyText.GetComponent<Text>().color = new Color32(26, 215, 115, 192);
        readyText.inputText = "READY";
        readyText.delay = 0;

        yield return new WaitForSeconds(0.5f);
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

        timerText.gameObject.SetActive(true);
        floorText.gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);
        effectTimerText.gameObject.SetActive(false);

        GameTimer.Instance.playTime = 1;

        clearTransparency = 0.25f;
        Debug.Log("SE_FloorStart");

        bandScreen.gameObject.SetActive(false);

    }
}
