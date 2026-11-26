using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpecialAction
{
    none,
    AllOver,
    Redemption
}

public class PlayerStatus : IGetMoveSpeed, IUseableRam, IDeckList, IPosition,
    IGetPlayerDie, ISetPlayerDied, IHaveGun, IGetPlayerScore, ISetScoreDestroy,
    IStatusSave, IRelicStatusEffect, ISetRelicList, IGetRelicList, ISetHitPoint,
    IGetHitPoint, ISetMoveSpeed, IGetSaveData, ISetPlayerSpecialAction, ISetMoneyNum,
    IGetMoneyNum, IGetTrace, ISetTrace, IFloorData
{

    public float Trace { get; private set; }

    public int HaveMoney { get; private set; }

    public int MaxHitPoint { get; private set; }
    public float NowHitPoint { get; private set; }

    public List<int> intRelicEvents { get; private set; }

    public List<IRelicEvent> relicEvents { get; private set; }

    public int ScoreDestroy { get; private set; }
    public int BulletMax { get; private set; }

    public int BulletNow { get; private set; }

    public GunName GunName { get; private set; }

    public GunData GunData { get; private set; }

    public int HaveGunNo { get; private set; }

    public event Action PlayerDie = delegate { };

    public Vector3 PlayerPosition { get; private set; }

    public float RamCapacity { get; }

    public float RamNow { get; private set; }

    public float RamRecovary { get; private set; }

    public bool IsReboot { get; private set; } = false;

    public int HandMaxSize { get; private set; }

    public float RebootTimer { get; private set; }

    private float baseMoveSpeed;

    private float plusMoveSpeed;

    public float MoveSpeed
    {
        get
        { return baseMoveSpeed + plusMoveSpeed; }
        private set
        { }
    }

    public List<int> DeckList { get; private set; }

    private List<SpecialAction> specialActionList = new();

    public float specialActionCount = 0;

    public float ShotTimer { get; private set; }
    public float ReloadTimer { get; private set; }

    public int NowFloor { get; private set; }

    public int SelectStageNo { get; private set; }


    public void PlayerPositionSet(Transform transform)
    {
        PlayerPosition = transform.position;
    }
    PlayerSaveData saveData;
    public PlayerStatus()
    {
        saveData = SaveManager.Instance.Load();

        // HP初期化
        MaxHitPoint = saveData.maxHitPoint;
        NowHitPoint = saveData.nowHitPoint;

        // Hack関連初期化
        RamCapacity = saveData.maxRamCapacity;
        RamRecovary = saveData.ramRecovery;
        HandMaxSize = saveData.maxHandSize;
        IsReboot = true;
        _ = RamUpdate();
        DeckList = saveData.deckList;

        baseMoveSpeed = saveData.moveSpeed;

        // 銃関連初期化
        GunName = saveData.gunName;

        ScoreDestroy = saveData.score_DestoryEnemy;

        intRelicEvents = saveData.relicEvents;

        relicEvents = new();

        HaveMoney = saveData.maney;
        Trace = saveData.trace;

        NowFloor = saveData.nowFloor;

        SelectStageNo = saveData.selectStageNo;

        ResetShotTimer();
    }

    public PlayerSaveData playerSave()
    {
        // HP初期化
        saveData.maxHitPoint = MaxHitPoint;
        saveData.nowHitPoint = NowHitPoint;

        // Hack関連初期化
        saveData.maxRamCapacity = RamCapacity;
        saveData.ramRecovery = RamRecovary;
        saveData.maxHandSize = HandMaxSize;
        saveData.deckList = DeckList;

        saveData.moveSpeed = baseMoveSpeed;

        // 銃関連初期化
        saveData.gunName = GunName;

        saveData.score_DestoryEnemy = ScoreDestroy;
        saveData.relicEvents = intRelicEvents;
        saveData.maney = HaveMoney;
        saveData.trace = Trace;
        saveData.nowFloor = NowFloor;
        return saveData;
    }

    public void RamUse(float useRam)
    {
        if (0 > RamNow - useRam)
        {
            if (specialActionList.Contains(SpecialAction.AllOver))
            {
                DamageHitPoint(RamNow - useRam * 10);
                RamNow = 0;
            }
            else
            {
                RamNow = 0;
                Debug.LogError("使える数より多いRAMを使おうとしてるよ！");
            }
        }
        else
        {
            RamNow -= useRam;
        }
    }

    public void RamChange(float addRam)
    {
        float ram = RamNow + addRam;
        // 上限、下限を超えないかチェック
        if (ram > RamCapacity)
        {
            RamNow = RamCapacity;
        }
        else if (ram < 0)
        {
            RamNow = 0;
        }
        else
        {
            RamNow = ram;
        }
    }

    public bool RamCanChange(int useRam)
    {
        if (0 > RamNow - useRam)
        {
            if (specialActionList.Contains(SpecialAction.AllOver))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public async UniTask RamUpdate()
    {
        while (true)
        {
            // IsRebootがtrueならRamRecovaryの時間まで待機して、Ramを初期値に戻す。
            if (IsReboot)
            {
                RamNow = 0;
                RebootTimer += GameTimer.Instance.GetScaledDeltaTime();

                if (RebootTimer >= RamRecovary)
                {
                    RamNow = RamCapacity;

                    RebootTimer = 0;
                    IsRebootSet(false);
                }
            }
            await UniTask.Yield();
        }
    }
    public void IsRebootSet(bool setBool)
    {
        IsReboot = setBool;
    }

    public void DieSet()
    {
        PlayerDie();
    }

    public void BulletUse()
    {
        int usedBullet = --BulletNow;
        if (usedBullet >= 0)
        {
            BulletNow = usedBullet;
        }
        else
        {
            BulletNow = 0;
            Debug.Log("残弾が下限突破してます！ UseBullet :" + usedBullet + "NowBullet :" + BulletNow);
        }
    }

    public void BulletSet(int _MaxBullet)
    {
        if (_MaxBullet == 0)
        {
            Debug.Log("最大の弾がゼロです！ MaxBullet :" + _MaxBullet + " GunName :" + GunName.ToString());
        }
        else
        {
            BulletMax = _MaxBullet;
            BulletNow = _MaxBullet;
        }
    }

    public void BulletAdd(int BulletAdd)
    {
        int BulletAdded = BulletNow + BulletAdd;
        if (BulletAdded <= BulletMax)
        {
            BulletNow = BulletAdded;
        }
        else
        {
            BulletNow = BulletMax;
            Debug.Log("最大値を超えて弾を追加しようとしてます！");
        }
    }
    public void BulletResume()
    {
        BulletNow = 0;
    }

    public void AddScoreDestroy(int addScore)
    {
        ScoreDestroy += addScore;
    }

    public int GetDestroyScore()
    {
        return ScoreDestroy;
    }

    public void MovePlus(int plusNum)
    {
        MoveSpeed += plusNum;
    }

    public void RamHeal(int healNum)
    {
        RamChange(healNum);
    }
    public void DeckDraw()
    {
        Debug.Log("ドロー！");
    }
    public void AddRelic(RelicName relicEffecter)
    {
        intRelicEvents.Add((int)relicEffecter);
    }

    public void RemoveRelic(RelicName relicEffecter)
    {
        intRelicEvents.Remove((int)relicEffecter);
    }

    public void DamageHitPoint(float dmg)
    {
        Debug.Log(dmg);
        NowHitPoint -= dmg;
        if (NowHitPoint <= 0)
        {
            DieSet();
        }
    }

    public void HealNowHitPoint(int healPoint)
    {
        if (MaxHitPoint <= NowHitPoint + healPoint)
        {
            NowHitPoint = MaxHitPoint;
        }
        else
        {
            NowHitPoint += healPoint;
        }
    }

    private float deadLineTimer = 0;
    private float deadLine = 1;
    public void DeadLineDamage()
    {
        if (specialActionList.Contains(SpecialAction.Redemption))
        {
            if (deadLineTimer >= deadLine)
            {
                deadLineTimer = 0;
                DamageHitPoint(10);
            }
            else
            {
                deadLineTimer += GameTimer.Instance.GetScaledDeltaTime();
            }
        }
        else
        {
            DamageHitPoint(MaxHitPoint);
        }
    }
    public void MoveSpeedUp(float plusNum)
    {
        plusMoveSpeed += plusNum;
    }

    public PlayerSaveData GetPlayerSaveData()
    {
        return saveData;
    }

    public void AddSpecialAction(SpecialAction addAction)
    {
        specialActionList.Add(addAction);
    }

    public int GetMoneyNum()
    {
        return HaveMoney;
    }

    public void GetMoney(int plusMoney)
    {
        HaveMoney += plusMoney;
    }
    public void UseMoney(int useMoney)
    {
        HaveMoney -= useMoney;
    }

    public bool CanUseMoney(int useMoney)
    {
        if (HaveMoney - useMoney > 0)
        {
            return true;
        }
        else
        {
            Debug.Log("お金なくて買えないよ！一昨日きやがれ");
            return false;
        }
    }

    public float GetTrace()
    {
        return Trace;
    }

    public void AddTrace(float addTracce)
    {
        Trace += addTracce;
    }

    public void HitPointInit()
    {
        NowHitPoint = MaxHitPoint;
    }

    public void ShotIntervalTimer()
    {
        ShotTimer += GameTimer.Instance.GetScaledDeltaTime();
    }
    public void ReloadIntervalTimer()
    {
        ReloadTimer += GameTimer.Instance.GetScaledDeltaTime();
    }
    public void ResetShotTimer()
    {
        ShotTimer = 0;
    }

    public void ResetReloadTimer()
    {
        ReloadTimer = 0;
    }

    public void AddNowFloor()
    {
        NowFloor++;
    }

    public void SelectStage(int num)
    {
        SelectStageNo = num;
    }
}
public interface IPosition
{
    public Vector3 PlayerPosition { get; }

    public void PlayerPositionSet(Transform transform);
}

public interface IGetMoveSpeed
{
    public float MoveSpeed { get; }
}

public interface IUseableRam
{
    public float RamCapacity { get; }

    public float RamNow { get; }

    public float RamRecovary { get; }

    public bool IsReboot { get; }

    public float RebootTimer { get; }
    public void RamUse(float useRam);

    public void RamChange(float addRam);

    public int HandMaxSize { get; }

    public UniTask RamUpdate();

    public void IsRebootSet(bool setBoll);
}

public interface IDeckList
{
    public List<int> DeckList { get; }
}

public interface IGetHelth
{
    public float MaxHitPointGet();
    public float NowHitPointGet();
}

public interface IHaveGun
{
    public GunName GunName { get; }

    public GunData GunData { get; }

    public int BulletMax { get; }

    public int BulletNow { get; }

    public float ShotTimer { get; }
    public float ReloadTimer { get; }

    public void BulletUse();

    public void BulletSet(int maxBullet);

    public void BulletAdd(int BulletAdd);

    public void BulletResume();

    public void ShotIntervalTimer();
    public void ReloadIntervalTimer();

    public void ResetShotTimer();
    public void ResetReloadTimer();
}

public interface IGetPlayerDie
{
    public event Action PlayerDie;
}

public interface ISetPlayerDied
{
    public void DieSet();
}

public interface IGetPlayerScore
{
    public int GetDestroyScore();
}

public interface ISetScoreDestroy
{
    public int ScoreDestroy { get; }

    public void AddScoreDestroy(int addScore);
}

public interface IStatusSave
{
    public PlayerSaveData playerSave();


    public void HitPointInit();
}

public interface IRelicStatusEffect
{
    public void MovePlus(int moveSpeedPlus);
    public void RamHeal(int ramHeal);
    public void DeckDraw();
}

public interface ISetRelicList
{
    public void AddRelic(RelicName relic);
    public void RemoveRelic(RelicName relic);
}

public interface IGetRelicList
{
    public List<IRelicEvent> relicEvents { get; }
    public List<int> intRelicEvents { get; }
}
public interface IGetHitPoint
{
    public float NowHitPoint { get; }
    public int MaxHitPoint { get; }
}

public interface ISetHitPoint
{
    public void DamageHitPoint(float dmg);
    public void HealNowHitPoint(int healPoint);
    public void DeadLineDamage();
}

public interface ISetMoveSpeed
{
    public void MoveSpeedUp(float moveSpeed);
}

public interface IGetSaveData
{
    public PlayerSaveData GetPlayerSaveData();
}

public interface ISetPlayerSpecialAction
{
    public void AddSpecialAction(SpecialAction specialAction);
}

public interface IGetMoneyNum
{
    public int GetMoneyNum();
}

public interface ISetMoneyNum
{
    public int HaveMoney { get; }

    public void GetMoney(int plusMoney);
    public void UseMoney(int useMoney);

    public bool CanUseMoney(int useMoney);
}

public interface ISetTrace
{
    public float Trace { get; }

    public void AddTrace(float addTrace);
}

public interface IGetTrace
{
    public float GetTrace();
}

public interface IFloorData
{
    public int NowFloor { get; }

    public int SelectStageNo { get; }

    public void AddNowFloor();

    public void SelectStage(int num);
}
