using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeChoice : MonoBehaviour
{
    [SerializeField]
    ICardType nowCard;

    // シーン名
    [SerializeField]
    string[] nextScene;

    bool nextSceneFrag = false;
    float timer;
    [SerializeField]
    float changeTime = 1;
    PlayerSaveData data;
    [SerializeField, Header("upgradeData")]
    UpGradeCardData upGradeCardData;
    // 重みの総和（初期化時に計算される）
    private float _totalWeight;
    private void Start()
    {
        data = SaveManager.Instance.Load();

        for (int i = 0; i < upGradeCardData.upGradeCardList.Count; i++)
        {
            var iCard = upGradeCardData.upGradeCardList[i].cardType.GetComponent<ICardType>();
            iCard.cardLevel = upGradeCardData.upGradeCardList[i].CardLevel;
            // ステージがすすむごとに出る重みを変える。
            if (data.score_Stage <= 5)
            {
                switch (iCard.cardLevel)
                {
                    case 1:
                        iCard.cardWeight = 20;
                        break;
                    case 2:
                        iCard.cardWeight = 5;
                        break;
                    case 3:
                        iCard.cardWeight = 0;
                        break;
                }
            }
            else if (data.score_Stage <= 10)
            {
                switch (iCard.cardLevel)
                {
                    case 1:
                        iCard.cardWeight = 20;
                        break;
                    case 2:
                        iCard.cardWeight = 20;
                        break;
                    case 3:
                        iCard.cardWeight = 10;
                        break;
                }
            }
            else
            {
                switch (iCard.cardLevel)
                {
                    case 1:
                        iCard.cardWeight = 0;
                        break;
                    case 2:
                        iCard.cardWeight = 10;
                        break;
                    case 3:
                        iCard.cardWeight = 20;
                        break;
                }
            }
            _totalWeight += iCard.cardWeight;
        }

        float x = -5;

        for (int i = 0; i < 3; i++)
        {
            Instantiate(upGradeCardData.upGradeCardList[Choose()].cardType, new Vector3(x, 0, 0), Quaternion.identity);
            x += 5;
        }
    }
    private void FixedUpdate()
    {

    }
    public int Choose()
    {
        // 0～重みの総和の範囲の乱数値取得
        var randomPoint = Random.Range(0, _totalWeight);

        // 乱数値が属する要素を先頭から順に選択
        var currentWeight = 0f;
        for (var i = 0; i < upGradeCardData.upGradeCardList.Count; i++)
        {
            // 現在要素までの重みの総和を求める
            currentWeight += upGradeCardData.upGradeCardList[i].cardType.GetComponent<ICardType>().cardWeight;

            // 乱数値が現在要素の範囲内かチェック
            if (randomPoint < currentWeight)
            {
                return i;
            }
        }

        // 乱数値が重みの総和以上なら末尾要素とする
        return upGradeCardData.upGradeCardList.Count - 1;
    }
    private void Update()
    {
        // デバッグ用
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // カード取得したかどうか
        if (nextSceneFrag)
        {
            timer += Time.deltaTime;
            if (timer > changeTime)
            {
                int rand = Random.Range(0, nextScene.Length);
                Debug.Log(nextScene[rand].ToString());
                SceneManager.LoadScene(nextScene[rand]);
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
                    Debug.Log("カードレベル : " + nowCard.cardLevel+nowCard);

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
