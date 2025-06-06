using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class UnitCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 1;

    public static UnitCore Instance { get; private set; }

    void Awake()
    {
        // Singleton�`�F�b�N
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �d����h�~
            return;
        }

        Instance = this;
    }

    void Start()
    {
        MAXHP = 10;
        NowHP = MAXHP;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Die()
    {
        Destroy(gameObject);
        Instance = null;
    }
}
