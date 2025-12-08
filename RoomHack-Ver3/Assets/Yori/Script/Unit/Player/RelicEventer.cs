using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class RelicEventer : MonoBehaviour
{
    [SerializeField]
    List<IRelicEvent> relicEvents = new List<IRelicEvent>();
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

    [Inject]
    IUseableRam useableRam;
    public void Start()
    {
    }

   
    private void Update()
    {
        if (relicList != null)
        {
            foreach (var item in relicList.relicEvents)
            {
                if (item != null)
                {
                    Debug.Log(item);
                    item.RelicEventAction();
                }
            }
        }
    }
}
public enum RelicName
{
    none,
    destoryHPHeal,
    destoryRamHeal,
    destroyDeckDraw,
    halfHitPointMoveSpeedUp,
}

public interface IRelicEvent
{
    public RelicName relicName { get; }
    public void RelicEventAction();

    public bool RelicEventTrriger();
}

public class DestroyerEventBase : IRelicEvent
{
    public RelicName relicName { get; private set; }
    public IGetPlayerScore getScore { get; }
    public int nowScore { get; private set; }
    public DestroyerEventBase(IGetPlayerScore _getScore)
    {
        getScore = _getScore;
        nowScore = getScore.GetDestroyScore();
    }
    IRelicStatusEffect relicStatusEffect;
    public virtual void RelicEventAction()
    {
        if (RelicEventTrriger())
        {
            Debug.Log(relicName + " : が起動したよ");
        }
    }
    public bool RelicEventTrriger()
    {
        if (getScore.GetDestroyScore() > nowScore)
        {
            nowScore = getScore.GetDestroyScore();
            return true;
        }
        return false;
    }
}

public class HitPointHeal : DestroyerEventBase
{
    ISetHitPoint setHitPoint;
    public HitPointHeal(IGetPlayerScore _getScore,ISetHitPoint _setHitPoint) : base(_getScore)
    {
        setHitPoint = _setHitPoint;
    }

    public override void RelicEventAction()
    {
        if (RelicEventTrriger())
        {
            Debug.Log("HP回復！");
            setHitPoint.HealNowHitPoint(3);
        }
    }
}

public class RamHeal : DestroyerEventBase
{
    IUseableRam useableRam;
    public RamHeal(IGetPlayerScore _getScore,IUseableRam _usebleRam) : base(_getScore)
    {
        useableRam = _usebleRam;
    }

    public override void RelicEventAction()
    {
        if (RelicEventTrriger())
        {
            useableRam.RamChange(3);
        }
    }
}


public class DeckDraw : DestroyerEventBase
{
    public DeckDraw(IGetPlayerScore _getScore) : base(_getScore)
    {

    }

    public override void RelicEventAction()
    {
        if (RelicEventTrriger())
        {
            ToolManager.Instance.DeckDraw();
        }
    }
}

public class HalfHitPointEffectBase : IRelicEvent
{
    protected IGetHitPoint getHitPoint;
    public RelicName relicName { get; private set; }
    public HalfHitPointEffectBase(IGetHitPoint _getHitPoint)
    {
        getHitPoint = _getHitPoint;
    }

    public virtual void RelicEventAction()
    {
        if (RelicEventTrriger())
        {
            Debug.Log(relicName + " : が起動したよ");
        }
    }
    public bool RelicEventTrriger()
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
    public override void RelicEventAction()
    {
        if (RelicEventTrriger())
        {
            // 仮の値
            setMoveSpeed.MoveSpeedUp(10);
        }
    }

}

