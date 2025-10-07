using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public List<GameObject> cardList = new List<GameObject>();

    [SerializeField]
    CardType nowCard;

    // シーン名
    [SerializeField]
    string nextScene;

    bool nextSceneFrag = false;
    float timer;
    [SerializeField]
    float changeTime = 1;
    PlayerSaveData data;
    private void Start()
    {

        data = SaveManager.Instance.Load();

        // 反映


        float x = -5;

        for (int i = 0; i < 3; i++)
        {
            Instantiate(cardList[Random.Range(0, cardList.Count)], new Vector3(x, 0, 0), Quaternion.identity);
            x += 5;
        }
    }

    private void Update()
    {
        // カード取得したかどうか
        if (nextSceneFrag)
        {
            timer += Time.deltaTime;
            if (timer > changeTime)
            {
                Debug.Log("シーン移動だお！");
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {
            // カード取得してないなら
            // マウスポジションにレイ投擲
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(MousePos(), Vector2.zero))
            {
                Debug.Log("あたったお");

                // あたったオブジェクトのカードタイプ取得
                if (hit.collider.TryGetComponent<CardType>(out nowCard))
                {
                    Debug.Log("これカードだお : " + nowCard);

                    // クリックでカードゲット
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("カードゲットだお : " + nowCard.card);
                        data.score_Stage++;

                        // 効果適用
                        if (nowCard.card == CardType.Card.BP)
                        {
                            //data.plusBreachPower += 2;
                        }

                        if (nowCard.card == CardType.Card.MH)
                        {
                            data.pulusMaxHitpoint += 20;
                        }

                        if (nowCard.card == CardType.Card.MS)
                        {
                            data.plusMoveSpeed += 0.5f;
                        }

                        if (nowCard.card == CardType.Card.RC)
                        {
                            data.plusRamCapacity += 1;
                        }

                        if (nowCard.card == CardType.Card.RR)
                        {
                            data.plusRamRecovery += 0.2f;
                        }
                        // 保存
                        SaveManager.Instance.Save(data);
                        nextSceneFrag = true;
                    }
                }
            }
        }
    }

    private Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
}
