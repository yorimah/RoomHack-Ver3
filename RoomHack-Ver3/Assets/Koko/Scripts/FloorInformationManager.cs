using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections;

public class FloorInformationManager : MonoBehaviour
{
    [Inject]
    IGetFloorData floorData;

    [SerializeField]
    GeneralUpdateText updateText;

    [SerializeField]
    Text timerText;

    public int floorNum = 0;

    private void Start()
    {
        EffectManager.Instance.ActEffect(EffectManager.EffectType.Time, new Vector2(0, 6));

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
        updateText.inputText = "FLOOR " + floorNum;
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
