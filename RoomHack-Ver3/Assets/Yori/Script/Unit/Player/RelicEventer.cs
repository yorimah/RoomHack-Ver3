using UnityEngine;
using Zenject;
public class RelicEventer : MonoBehaviour
{

    [Inject]
    IGetRelicList relicList;

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
                    if (item.RelicEventTrigger())
                    {
                        item.RelicEventAction();
                    }
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

    public bool RelicEventTrigger();

    public bool IsEventTrigger { get; }
}

public class DestroyerEventBase : IRelicEvent
{
    public RelicName relicName { get; private set; }
    public IGetPlayerScore getScore { get; }
    public int nowScore { get; private set; }
    
    public bool IsEventTrigger { get;private set; }
    public DestroyerEventBase(IGetPlayerScore _getScore, RelicName _relicName)
    {
        getScore = _getScore;
        nowScore = getScore.GetDestroyScore();
        relicName = _relicName;
    }
    IRelicStatusEffect relicStatusEffect;
    public virtual void RelicEventAction()
    {
        Debug.Log(relicName + " : が起動したよ");
    }
    public bool RelicEventTrigger()
    {
        if (getScore.GetDestroyScore() > nowScore)
        {
            nowScore = getScore.GetDestroyScore();
            IsEventTrigger = true;
            return true;
        }
        return false;
    }
}

public class NoneRelic : IRelicEvent
{

    public RelicName relicName { get; } = RelicName.none;

    public bool IsEventTrigger { get; private set; }
    public void RelicEventAction()
    {

    }

    public bool RelicEventTrigger()
    {
        return false;
    }

}


public class HitPointHeal : DestroyerEventBase
{
    ISetHitPoint setHitPoint;
    public HitPointHeal(IGetPlayerScore _getScore, ISetHitPoint _setHitPoint, RelicName _relicName) : base(_getScore, _relicName)
    {
        setHitPoint = _setHitPoint;
    }

    public override void RelicEventAction()
    {
        Debug.Log("HP回復！");
        setHitPoint.HealNowHitPoint(3);
    }
}

public class RamHeal : DestroyerEventBase
{
    IUseableRam useableRam;
    public RamHeal(IGetPlayerScore _getScore, IUseableRam _usebleRam, RelicName _relicName) : base(_getScore, _relicName)
    {
        useableRam = _usebleRam;
    }

    public override void RelicEventAction()
    {
        useableRam.RamChange(3);
    }
}


public class DeckDraw : DestroyerEventBase
{
    public DeckDraw(IGetPlayerScore _getScore, RelicName relicName) : base(_getScore, relicName)
    {

    }

    public override void RelicEventAction()
    {
        ToolManager.Instance.DeckDraw();
    }
}

public class HalfHitPointEffectBase : IRelicEvent
{
    protected IGetHitPoint getHitPoint;
    public RelicName relicName { get; private set; }

    public bool IsEventTrigger { get; private set; }
    public HalfHitPointEffectBase(IGetHitPoint _getHitPoint, RelicName relicName)
    {
        this.relicName = relicName;
        getHitPoint = _getHitPoint;
    }

    public virtual void RelicEventAction()
    {
        if (RelicEventTrigger())
        {
            Debug.Log(relicName + " : が起動したよ");
        }
    }
    public bool RelicEventTrigger()
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
    public HalfMoveSpeed(IGetHitPoint _getHitPoint, ISetMoveSpeed _setMoveSpeed, RelicName relicName) : base(_getHitPoint, relicName)
    {
        setMoveSpeed = _setMoveSpeed;
    }
    public override void RelicEventAction()
    {
        setMoveSpeed.MoveSpeedUp(10);
    }

}

