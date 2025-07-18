using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy : MonoBehaviour, IDamegeable, IHackObject
{
    // ハック関連
    public int secLevele { set; get; }

    public bool clacked { get; set; }

    public float MaxFireWall { get; set; }
    public float NowFireWall { get; set; }

    public float FireWallCapacity { get; set; }

    public float FireWallRecovaryNum { get; set; }

    // ダメージ関連
    public float MAXHP { get; set; } = 5;
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 2;

    protected IState currentState;

    [SerializeField, Header("動く速さ")]
    public float moveSpeed = 3f;

    [SerializeField, Header("障害物に使うレイヤー")]
    private LayerMask obstacleMask;

    [SerializeField, Header("GunData")]
    public GunData gundata;

    [SerializeField, Header("HP")]
    private float MaxHP;
    public PlayerCheack playerCheack;

    [HideInInspector]
    public float aimTime = 0.5f;
    [HideInInspector]
    public int MaxMagazine;
    [HideInInspector]
    public int nowMagazine;
    [HideInInspector]
    public int shotRate;
    [HideInInspector]
    public float bulletSpeed;
    public enum StateType
    {
        Idle,
        Move,
        Shot,
        Reload,
        Die,
        Clack,
        num
    }

    public void Awake()
    {
        MAXHP = MaxHP;
        NowHP = MAXHP;
        shotRate = gundata.rate;
        MaxMagazine = gundata.MaxMagazine;
        nowMagazine = MaxMagazine;
        bulletSpeed = gundata.bulletSpeed;
    }

    public StateType statetype;
    public Dictionary<StateType, IState> states;

    void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(StateType type)
    {
        currentState?.Exit();
        currentState = states[type];
        currentState?.Enter();
    }

    public LayerMask GetObstacleMask()
    {
        return obstacleMask;
    }

    public void Die()
    {
        ChangeState(StateType.Die);
    }

    public void CapacityOver()
    {
        ChangeState(StateType.Clack);
    }
    public void FireWallRecavary()
    {
        NowFireWall++;
        
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;
        Handles.Label(transform.position + Vector3.up * 1f, "HP " + NowHP.ToString(), style);
        Handles.Label(transform.position + Vector3.up * 1.5f, "残弾 " + nowMagazine.ToString(), style);
        if (currentState != null)
        {
            Handles.Label(transform.position + Vector3.up * 2f, "実行ステート " + currentState.ToString(), style);
        }

    }
#endif
}
