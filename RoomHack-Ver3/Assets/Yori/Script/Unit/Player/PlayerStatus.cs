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

public class PlayerStatus : IReadOnlyMoveSpeed, IUseableRam, IDeckList, IPosition,
    IGetPlayerDie, ISetPlayerDied, IGetMaxHitPoint, IHaveGun
{
    public int BulletMax { get; private set; }

    public int BulletNow { get; private set; }

    public GunName GunName { get; private set; }

    public GunData GunData { get; private set; }

    public int HaveGunNo { get; private set; }

    public event Action PlayerDie = delegate { };

    public Vector3 PlayerPosition { get; private set; }

    public int maxHitPoint { get; private set; }

    public float hitPointNow;

    public float RamCapacity { get; }

    public float RamNow { get; private set; }

    public float RamRecovary { get; private set; }

    public bool IsReboot { get; private set; } = false;

    public int HandMaxSize { get; private set; }

    public float RebootTimer { get; private set; }

    public float MoveSpeed { get; }

    public List<int> DeckList { get; private set; }

    private SpecialAction SpecialAction = SpecialAction.none;

    public float specialActionCount = 0;


    public void PlayerPositionSet(Transform transform)
    {
        PlayerPosition = transform.position;
    }

    public PlayerStatus()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();

        // HP初期化
        maxHitPoint = saveData.maxHitPoint;
        hitPointNow = saveData.maxHitPoint;

        // Hack関連初期化
        RamCapacity = saveData.maxRamCapacity;
        RamNow = saveData.maxRamCapacity; ;
        RamRecovary = saveData.ramRecovery;
        HandMaxSize = saveData.maxHandSize;
        IsReboot = true;
        _ = RamUpdate();
        DeckList = saveData.deckList;

        MoveSpeed = saveData.moveSpeed;

        // 銃関連初期化
        GunName = saveData.gunName;

    }

    public void RamUse(float useRam)
    {
        if (0 >= RamNow - useRam)
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
        if (ram <= RamCapacity && ram >= 0)
        {
            RamNow += addRam;
        }
        else
        {
            Debug.LogError("RamAddで上限、下限を超えました");
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
            Debug.LogError("最大の弾がゼロです！ MaxBullet :" + _MaxBullet);
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
}

public interface IGetMaxHitPoint
{
    public int maxHitPoint { get; }
}
public interface IPosition
{
    public Vector3 PlayerPosition { get; }

    public void PlayerPositionSet(Transform transform);
}

public interface IReadOnlyMoveSpeed
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