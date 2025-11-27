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

    [SerializeField]
    GeneralUpdateText updateText;

    [SerializeField]
    Text timerText;

    private void Start()
    {
        EffectManager.Instance.ActEffect(EffectManager.EffectType.Time, new Vector2(0, 6));

        saveData = getSaveData.GetPlayerSaveData();

        StartCoroutine(StartSequence());
    }

    private void Update()
    {
        if (floorData.isClear)
        {
            timerText.gameObject.SetActive(false);
            updateText.gameObject.SetActive(true);
            updateText.inputText = "FLOOR CLEAR";
            this.transform.position = new Vector2(0, -20);
        }
    }

    IEnumerator StartSequence()
    {
        timerText.gameObject.SetActive(false);
        updateText.gameObject.SetActive(true);
        updateText.inputText = "FLOOR " + saveData.nowFloor;
        this.transform.position = new Vector2(0, 20);

        GameTimer.Instance.playTime = 0;

        yield return new WaitForSeconds(1);

        this.transform.position = Vector2.zero;

        yield return new WaitForSeconds(1);


        timerText.gameObject.SetActive(true);
        updateText.gameObject.SetActive(false);

        GameTimer.Instance.playTime = 1;
    }
}
