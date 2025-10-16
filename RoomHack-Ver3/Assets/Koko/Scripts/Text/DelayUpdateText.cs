using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DelayUpdateText : MonoBehaviour
{
    [SerializeField]
    float delay = 0;

    string inputText;
    Text dispText;

    private void Awake()
    {
        dispText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(TextUpdate());
    }


    IEnumerator TextUpdate()
    {
        inputText = dispText.text;
        dispText.text = null;

        foreach (var item in inputText)
        {
            dispText.text += item;
            for (int i = 0; i < delay; i++)
            {
                yield return new WaitForEndOfFrame();
            }
        }

    }

}
