using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RamView : MonoBehaviour
{
    [Inject]
    IUseableRam ram;

    Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        text.text = "MaxRam : " + ram.RamCapacity;
    }
}
