using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
public class GameManeger : MonoBehaviour
{
    private List<Enemy> eList;

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
        ClearCheck();
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
        SaveManager.Instance.Save(statusSave.playerSave());
        SceneManager.LoadScene("UpgradeTest");
    }
}
