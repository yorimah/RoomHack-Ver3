using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
public class GameManager : MonoBehaviour
{
    private List<Enemy> eList;

    [Inject]
    IGetCleaFlag getFloorData;

    [Inject]
    IGetEnemyList getEnemyList;

    [Inject]
    IStatusSave statusSave;

    void Start()
    {
        eList = getEnemyList.GetEnemies();
    }

    private void Update()
    {
        if (!getFloorData.isClear) ClearCheck();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player") ClearCheck();
    }

    void ClearCheck()
    {
        foreach (var enemy in eList)
        {
            if (!enemy.isDead)
            {
                return;
            }
        }

        StartCoroutine(ClearSequence());
        getFloorData.SetClear();
    }

    IEnumerator ClearSequence()
    {

        yield return new WaitForSeconds(0.5f);
        GameTimer.Instance.playTime = 0;
        //GameTimer.Instance.PauseGame();

        //for (int i = 0; i < 10; i++)
        //{
        //    GameTimer.Instance.SetCustumTimeScale(1 - i*0.1f);
        //    yield return new WaitForSeconds(0.1f);
        //}

        yield return new WaitForSeconds(1.5f);
        SaveManager.Instance.Save(statusSave.playerSave());
        SceneManager.LoadScene("ToolGetScene");
    }


}


public interface IGetCleaFlag
{
    public bool isClear { get; }

    public void SetClear();
}

public class ClearManager:IGetCleaFlag
{
    public bool isClear { get; private set; }
    
    public void SetClear()
    {
        isClear = true;
    }
}
