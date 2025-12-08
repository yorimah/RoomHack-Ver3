using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

enum SpecialAction
{
    none,
    EdgeRun,
    Blink
}

public class PlayerStatus : IGetMoveSpeed, IUseableRam, IDeckList, IPosition,
    IGetPlayerDie, ISetPlayerDied, IHaveGun, IGetPlayerScore, ISetScoreDestroy,
    IStatusSave, IRelicStatusEffect, ISetRelicList, IGetRelicList, ISetHitPoint, IGetHitPoint
    , ISetMoveSpeed, IGetSaveData
{

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

    private SpecialAction SpecialAction = SpecialAction.none;

    public float specialActionCount = 0;


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
        NowHitPoint = saveData.maxHitPoint;

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

        intRelicEvents = saveData.relicEffecters;

        relicEvents = new();

        foreach (var intRelicEvent in intRelicEvents)
        {
            relicEvents.Add(RelicIns((RelicName)intRelicEvent));
        }
    }

    public PlayerSaveData playerSave()
    {
        // HP初期化
        saveData.maxHitPoint = MaxHitPoint;

        // Hack関連初期化
        saveData.maxRamCapacity = RamCapacity;
        saveData.ramRecovery = RamRecovary;
        saveData.maxHandSize = HandMaxSize;
        saveData.deckList = DeckList;

        saveData.moveSpeed = baseMoveSpeed;

        // 銃関連初期化
        saveData.gunName = GunName;

        saveData.score_DestoryEnemy = ScoreDestroy;
        saveData.relicEffecters = intRelicEvents;
        return saveData;
    }

    public void RamUse(float useRam)
    {
        if (0 > RamNow - useRam)
        {
            RamNow = 0;
            Debug.LogError("使える数より多いRAMを使おうとしてるよ！");
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
            Debug.LogError("残弾が下限突破してます！ UseBullet :" + usedBullet + "NowBullet :" + BulletNow);
        }
    }

    public void BulletSet(int _MaxBullet)
    {
        if (_MaxBullet == 0)
        {
            Debug.LogError("最大の弾がゼロです！ MaxBullet :" + _MaxBullet + " GunName :" + GunName.ToString());
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
            Debug.LogError("最大値を超えて弾を追加しようとしてます！");
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
        NowHitPoint -= dmg;
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

    public void MoveSpeedUp(float plusNum)
    {
        plusMoveSpeed = plusNum;
    }

    public PlayerSaveData GetPlayerSaveData()
    {
        return saveData;
    }

    public IRelicEvent RelicIns(RelicName relicName)
    {
        switch (relicName)
        {
            case RelicName.none:
                return null;
            case RelicName.destoryHPHeal:
                return new HitPointHeal(this, this);
            case RelicName.destoryRamHeal:
                return new RamHeal(this, this);
            case RelicName.destroyDeckDraw:
                return new DeckDraw(this);
            case RelicName.halfHitPointMoveSpeedUp:
                return new HalfMoveSpeed(this, this);
        }
        return null;
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

    public void BulletUse();

    public void BulletSet(int maxBullet);

    public void BulletAdd(int BulletAdd);

    public void BulletResume();
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
}

public interface ISetMoveSpeed
{
    public void MoveSpeedUp(float moveSpeed);
}

public interface IGetSaveData
{
    public PlayerSaveData GetPlayerSaveData();
}