using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDebbuger : MonoBehaviour
{
    public Text debugTextUI; // UI Text にアタッチ
    private Queue<string> stateLogs = new Queue<string>();
    private const int maxLogs = 10;

    private static StageDebbuger _instance;
    public static StageDebbuger Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("StateDebugger");
                _instance = go.AddComponent<StageDebbuger>();

                Canvas canvas = new GameObject("Canvas").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.transform.SetParent(go.transform);

                GameObject textObj = new GameObject("DebugText");
                textObj.transform.SetParent(canvas.transform);
                Text text = textObj.AddComponent<Text>();
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.color = Color.green;
                text.fontSize = 16;
                text.alignment = TextAnchor.UpperLeft;
                RectTransform rt = text.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(500, 300);
                rt.anchoredPosition = new Vector2(10, -10);

                _instance.debugTextUI = text;
            }
            return _instance;
        }
    }

    public void LogStateChange(string fromState, string toState)
    {
        string log = $"[{Time.time:F2}s] {fromState} → {toState}";
        if (stateLogs.Count >= maxLogs) stateLogs.Dequeue();
        stateLogs.Enqueue(log);
        UpdateText();
    }

    private void UpdateText()
    {
        debugTextUI.text = string.Join("\n", stateLogs.ToArray());
    }
}
