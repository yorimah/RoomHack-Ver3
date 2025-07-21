using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
public class UnitCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 1;

    public static UnitCore Instance { get; private set; }

    SaveManager saveManager;
    PlayerSaveData data;
    void Awake()
    {
        saveManager = new();
        // Singletonチェック
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複を防止
            return;
        }

        Instance = this;
    }

    void Start()
    {
        MAXHP = 100;
        NowHP = MAXHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        saveManager.Save(data);
        SceneManager.LoadScene("GameOverDemoScene");
        Destroy(gameObject);
    }
}
