using System.Collections;
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

    private void Start()
    {
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

                        // 効果適用
                        if (nowCard.card == CardType.Card.BP)
                        {

                        }

                        if (nowCard.card == CardType.Card.MH)
                        {

                        }

                        if (nowCard.card == CardType.Card.MS)
                        {

                        }

                        if (nowCard.card == CardType.Card.RC)
                        {

                        }

                        if (nowCard.card == CardType.Card.RR)
                        {

                        }

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
