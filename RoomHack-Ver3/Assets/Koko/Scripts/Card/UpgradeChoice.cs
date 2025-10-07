using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeChoice : MonoBehaviour
{
    public List<GameObject> cardList = new List<GameObject>();

    [SerializeField]
    ICardType nowCard;

    // シーン名
    [SerializeField]
    string nextScene;

    bool nextSceneFrag = false;
    float timer;
    [SerializeField]
    float changeTime = 1;
    PlayerSaveData data;

    // 各要素の重みリスト
    private float[] _weights;

    // 重みの総和（初期化時に計算される）
    private float _totalWeight;
    private void Start()
    {

        data = SaveManager.Instance.Load();

        for (int i = 0; i < cardList.Count; i++)
        {
            _weights[i] = cardList[i].GetComponent<ICardType>().cardWeight;
        }

        float x = -5;

        for (int i = 0; i < 3; i++)
        {

            Instantiate(cardList[Choose()], new Vector3(x, 0, 0), Quaternion.identity);
            x += 5;
        }
    }

    public int Choose()
    {
        // 0～重みの総和の範囲の乱数値取得
        var randomPoint = Random.Range(0, _totalWeight);

        // 乱数値が属する要素を先頭から順に選択
        var currentWeight = 0f;
        for (var i = 0; i < _weights.Length; i++)
        {
            // 現在要素までの重みの総和を求める
            currentWeight += _weights[i];

            // 乱数値が現在要素の範囲内かチェック
            if (randomPoint < currentWeight)
            {
                return i;
            }
        }

        // 乱数値が重みの総和以上なら末尾要素とする
        return _weights.Length - 1;
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
                if (hit.collider.TryGetComponent<ICardType>(out nowCard))
                {
                    Debug.Log("これカードだお : " + nowCard);

                    // クリックでカードゲット
                    if (Input.GetMouseButtonDown(0))
                    {
                        data.score_Stage++;
                        // 選択したカードの効果起動
                        data = nowCard.Choiced(data);
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
