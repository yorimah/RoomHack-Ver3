using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
public class GameManager : MonoBehaviour
{
    private List<Enemy> eList;

    private bool isClear = false;

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
        if (!isClear) ClearCheck();
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
        isClear = true;
    }

    IEnumerator ClearSequence()
    {


        yield return new WaitForSeconds(1);
        SaveManager.Instance.Save(statusSave.playerSave());
        SceneManager.LoadScene("UpgradeTest");
    }
}
