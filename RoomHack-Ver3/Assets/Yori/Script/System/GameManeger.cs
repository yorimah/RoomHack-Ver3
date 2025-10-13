using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManeger : MonoBehaviour
{
    PlayerSaveData data;

    [SerializeField, Header("敵をいれてね")]
    List<Enemy> eList;

    int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        data = SaveManager.Instance.Load();
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
        if (enemyCount <= 0 && collision.gameObject == UnitCore.Instance.gameObject)
        {

            Debug.Log("クリア");
            SaveManager.Instance.Save(data);
            SceneManager.LoadScene("UpgradeTest");
        }
    }
}
