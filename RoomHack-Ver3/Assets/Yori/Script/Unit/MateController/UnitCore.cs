using UnityEngine;
#if UNITY_EDITOR
#endif
using UnityEngine.SceneManagement;
public class UnitCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 1;

    public static UnitCore Instance { get; private set; }

    private PlayerSaveData data;

    public float ramCapacity;
    public float nowRam;
    public float ramRecovary;


    private int initMaxHp = 100;
    private int initRamCapacity = 10;
    private int initRamRecovary = 1;

    void Awake()
    {
        data = SaveManager.Instance.Load();

        MAXHP = initMaxHp + data.pulusMaxHitpoint;
        NowHP = MAXHP;
        ramCapacity = initRamCapacity + data.plusRamCapacity;
        nowRam = ramCapacity;
        ramRecovary = initRamRecovary + data.plusRamRecovery;


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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        SceneManager.LoadScene("GameOverDemoScene");
    }
}
