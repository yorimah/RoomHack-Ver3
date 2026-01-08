using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class MoneyView : MonoBehaviour
{
    [Inject]
    IGetMoneyNum getMoneyNum;

    private int money = 0;

    [SerializeField, Header("残金を表示するテキスト")]
    private Text moneyText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyText = moneyText.GetComponent<Text>();
        moneyText.text = "Money : " + money;
    }

    // Update is called once per frame
    void Update()
    {
        if (money != getMoneyNum.GetMoneyNum())
        {
            money = getMoneyNum.GetMoneyNum();
            moneyText.text = "Money : " + money;
        }
    }
}
