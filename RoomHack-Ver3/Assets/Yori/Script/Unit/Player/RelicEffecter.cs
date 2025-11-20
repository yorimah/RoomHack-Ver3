using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class RelicEffecter : MonoBehaviour
{
    [SerializeField]
    List<IRelicEffecter> relicEffecters = new List<IRelicEffecter>();
    [Inject]
    IGetPlayerScore getScore;

    [Inject]
    IGetRelicList relicList;

    [Inject]
    ISetMoveSpeed setMoveSpeed;

    [Inject]
    IGetHitPoint getHitPoint;

    [Inject]
    ISetHitPoint setHitPoint;
    public void Start()
    {
        foreach (var relic in relicList.relicEffecters)
        {
            relicEffecters.Add(RelicIns((RelicName)relic));
        }
    }

    public IRelicEffecter RelicIns(RelicName relicName)
    {
        switch (relicName)
        {
            case RelicName.destoryHPHeal:
                return new HitPointHeal(getScore, setHitPoint);
            case RelicName.destoryRamHeal:
                return new RamHeal(getScore);
            case RelicName.destroyDeckDraw:
                return new DeckDraw(getScore);
            case RelicName.halfHitPointMoveSpeedUp:
                return new HalfMoveSpeed(getHitPoint, setMoveSpeed);
        }
        return null;
    }
    private void Update()
    {
        if (relicEffecters != null)
        {
            foreach (var item in relicEffecters)
            {
                item.RelicEffect();
            }
        }
    }
}
public enum RelicName
{
    destoryHPHeal,
    destoryRamHeal,
    destroyDeckDraw,
    halfHitPointMoveSpeedUp,
}

public interface IRelicEffecter
{
    public RelicName relicName { get; }
    public void RelicEffect();

    public bool RelicEffectTrriger();
}

public class DestroyerEffectBase : IRelicEffecter
{
    public RelicName relicName { get; private set; }
    public IGetPlayerScore getScore { get; }
    public int nowScore { get; private set; }
    public DestroyerEffectBase(IGetPlayerScore _getScore)
    {
        getScore = _getScore;
        nowScore = getScore.GetDestroyScore();
    }
    IRelicStatusEffect relicStatusEffect;
    public virtual void RelicEffect()
    {
        if (RelicEffectTrriger())
        {
            Debug.Log(relicName + " : が起動したよ");
        }
    }
    public bool RelicEffectTrriger()
    {
        if (getScore.GetDestroyScore() > nowScore)
        {
            nowScore = getScore.GetDestroyScore();
            return true;
        }
        return false;
    }
}

public class HitPointHeal : DestroyerEffectBase
{
    ISetHitPoint setHitPoint;
    public HitPointHeal(IGetPlayerScore _getScore,ISetHitPoint _setHitPoint) : base(_getScore)
    {
        setHitPoint = _setHitPoint;
    }

    public override void RelicEffect()
    {
        if (RelicEffectTrriger())
        {
            Debug.Log("HP回復！");
            setHitPoint.HealNowHitPoint(3);
        }
    }
}

public class RamHeal : DestroyerEffectBase
{
    public RamHeal(IGetPlayerScore _getScore) : base(_getScore)
    {
    }

    public override void RelicEffect()
    {
        if (RelicEffectTrriger())
        {
            Debug.Log("Ram回復！");
        }
    }
}


public class DeckDraw : DestroyerEffectBase
{
    public DeckDraw(IGetPlayerScore _getScore) : base(_getScore)
    {

    }

    public override void RelicEffect()
    {
        if (RelicEffectTrriger())
        {
            ToolManager.Instance.DeckDraw();
        }
    }
}

public class HalfHitPointEffectBase : IRelicEffecter
{
    protected IGetHitPoint getHitPoint;
    public RelicName relicName { get; private set; }
    public HalfHitPointEffectBase(IGetHitPoint _getHitPoint)
    {
        getHitPoint = _getHitPoint;
    }

    public virtual void RelicEffect()
    {
        if (RelicEffectTrriger())
        {
            Debug.Log(relicName + " : が起動したよ");
        }
    }
    public bool RelicEffectTrriger()
    {
        if (getHitPoint.MaxHitPoint / 2 >= getHitPoint.NowHitPoint)
        {
            return true;
        }
        return false;
    }
}

public class HalfMoveSpeed : HalfHitPointEffectBase
{
    ISetMoveSpeed setMoveSpeed;
    public HalfMoveSpeed(IGetHitPoint _getHitPoint, ISetMoveSpeed _setMoveSpeed) : base(_getHitPoint)
    {
        setMoveSpeed = _setMoveSpeed;
    }
    public override void RelicEffect()
    {
        if (RelicEffectTrriger())
        {
            // 仮の値
            setMoveSpeed.MoveSpeedUp(10);
        }
    }

}

