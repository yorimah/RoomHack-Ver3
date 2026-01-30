using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HelthView : MonoBehaviour
{
    [Inject]
    IGetHitPoint getHitPoint;

    Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        text.text = "MaxHitPoint : " + getHitPoint.MaxHitPoint.ToString();
    }
}
