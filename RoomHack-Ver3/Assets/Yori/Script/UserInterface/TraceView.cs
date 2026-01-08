using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TraceView : MonoBehaviour
{
    [Inject]
    private IGetTrace getTrace;

    private float traceFloat;

    [SerializeField,Header("TraceText")]
    private Text traceText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        traceText = traceText.GetComponent<Text>();
        traceFloat = getTrace.GetTrace();
        traceText.text = "Trace : " + traceFloat.ToString("F1");
    }

    // Update is called once per frame
    void Update()
    {
        if (traceFloat != getTrace.GetTrace())
        {
            traceFloat = getTrace.GetTrace();
            traceText.text = "Trace : " + traceFloat.ToString("F1");
        }
    }
}
