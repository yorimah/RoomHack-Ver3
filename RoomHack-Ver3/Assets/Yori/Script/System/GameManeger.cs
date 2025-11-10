using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
public class GameManeger : MonoBehaviour
{
    PlayerSaveData data;

    private List<Enemy> eList;

    int enemyCount;
    [Inject]
    IGetEnemyList getEnemyList;

    void Start()
    {
        eList = getEnemyList.GetEnemies();
        data = SaveManager.Instance.Load();
        CheckDestroy();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDestroy();
    }

    void CheckDestroy()
    {
        enemyCount = eList.Count;
        for (int i = 0; i < eList.Count; i++)
        {
            if (eList[i].gameObject.activeSelf == false)
            {
                enemyCount--;
                data.score_DestoryEnemy++;
                SaveManager.Instance.Save(data);
                eList.RemoveAt(i);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyCount <= 0 && collision.name == "Player")
        {
            SaveManager.Instance.Save(data);
            SceneManager.LoadScene("UpgradeTest");
        }
    }
}
