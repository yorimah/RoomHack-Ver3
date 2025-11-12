using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CCTV : MonoBehaviour, IHackObject
{
    // ハッキング実装
    public List<toolTag> canHackToolTag { get; set; } = new List<toolTag>();
    public List<ToolEventBase> nowHackEvent { get; set; } = new List<ToolEventBase>();

    public bool CanHack { get; set; } = false;

    public bool IsView { get; set; }

    List<Mesh> mesh = new List<Mesh>();

    [SerializeField]
    GameObject triangle;

    List<GameObject> triangles = new List<GameObject>();

    [SerializeField]
    float rayRot = 360;
    [SerializeField]
    int rayValue = 36;
    [SerializeField]
    float rayLen = 5;
    [SerializeField]
    float rayOffset = 0;

    [SerializeField]
    LayerMask targetLm;
    List<Vector2> Items = new List<Vector2>();
    private GameObject viewMeshs;

    public CancellationToken token;
    CancellationTokenSource cts;
    private void Awake()
    {
        // HackData初期化
        canHackToolTag = new List<toolTag> { toolTag.CCTVHack };
    }



#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;

    }
#endif
}
