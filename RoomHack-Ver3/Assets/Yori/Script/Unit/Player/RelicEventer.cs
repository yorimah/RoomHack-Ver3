using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
public class RelicEventer : MonoBehaviour
{
    [Inject]
    IGetPlayerScore playerScore;

    [Inject]
    ISetMoveSpeed setMoveSpeed;

    [Inject]
    IGetRelicList relicList;

    [Inject]
    IGetHitPoint getHitPoint;
    [Inject]
    ISetHitPoint setHitPoint;
    [Inject]
    IUseableRam useableRam;
    [Inject]
    ISetPlayerSpecialAction setPlayerSpecialAction;

    public void Start()
    {
        foreach (var intRelicEvent in relicList.intRelicEvents)
        {
            relicList.relicEvents.Add(RelicIns((RelicName)intRelicEvent));
        }

    }


    private void Update()
    {
        if (relicList != null)
        {
            foreach (var item in relicList.relicEvents)
            {
                if (item != null)
                {
                    item.RelicEventAction();
                }
            }
        }
    }

    public IRelicEvent RelicIns(RelicName relicName)
    {
        switch (relicName)
        {
            case RelicName.none:
                return new NoneRelic();
            case RelicName.destoryHPHeal:
                return new HitPointHeal(playerScore, setHitPoint, relicName);
            case RelicName.destoryRamHeal:
                return new RamHeal(playerScore, useableRam, relicName);
            case RelicName.destroyDeckDraw:
                return new DeckDraw(playerScore, relicName);
            case RelicName.halfHitPointMoveSpeedUp:
                return new HalfMoveSpeed(getHitPoint, setMoveSpeed, relicName);
            case RelicName.allOverTheBurst:
                return new AllOverTheBurst(setPlayerSpecialAction, relicName);
            case RelicName.brawProtcol:
                return new BrawProtocal(playerScore, relicName, setMoveSpeed);
        }
        return null;
    }
}

public enum RelicName
{
    none,
    destoryHPHeal,
    destoryRamHeal,
    destroyDeckDraw,
    halfHitPointMoveSpeedUp,
    allOverTheBurst,
    brawProtcol,
}

public interface IRelicEvent
{
    public RelicName relicName { get; }
    public void RelicEventAction();

    public bool RelicEventTrigger();

    public bool IsEventTrigger { get; }
}

public interface IStackRelicEvent
{
    public int Stack { get; }
}

public class DestroyerEventBase : IRelicEvent
{
    public RelicName relicName { get; private set; }
    public IGetPlayerScore getScore { get; }
    public int nowScore { get; private set; }

    public bool IsEventTrigger { get; private set; }
    public DestroyerEventBase(IGetPlayerScore _getScore, RelicName _relicName)
    {
        getScore = _getScore;
        nowScore = getScore.GetDestroyScore();
        relicName = _relicName;
    }

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
            _ = EventTriggerDown();
            return true;
        }
        return false;
    }

    public async UniTask EventTriggerDown()
    {
        float timer = 0;
        while (IsEventTrigger)
        {
            timer += GameTimer.Instance.GetUnScaledDeltaTime();
            if (timer >= 0.5f)
            {
                IsEventTrigger = false;
            }
            await UniTask.Yield();
        }
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
        if (RelicEventTrigger())
        {
            setHitPoint.HealNowHitPoint(5);
        }
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
        if (RelicEventTrigger())
        {
            useableRam.RamChange(3);
        }
    }
}


public class DeckDraw : DestroyerEventBase
{
    public DeckDraw(IGetPlayerScore _getScore, RelicName relicName) : base(_getScore, relicName)
    {

    }

    public override void RelicEventAction()
    {
        if (RelicEventTrigger())
        {
            ToolManager.Instance.DeckDraw();
        }
    }
}

public class BrawProtocal : DestroyerEventBase, IStackRelicEvent
{
    public int Stack { get; private set; }
    private ISetMoveSpeed setMoveSpeed;
    public BrawProtocal(IGetPlayerScore _getScore, RelicName relicName, ISetMoveSpeed _setMoveSpeed) : base(_getScore, relicName)
    {
        Stack = 0;
        setMoveSpeed = _setMoveSpeed;
    }
    public override void RelicEventAction()
    {
        if (RelicEventTrigger())
        {
            Stack++;
            setMoveSpeed.MoveSpeedUp(0.2f);
        }
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
            IsEventTrigger = true;
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
        if (RelicEventTrigger() && !IsEventTrigger)
        {
            setMoveSpeed.MoveSpeedUp(2.5f);
        }
    }

}
public class AllOverTheBurst : IRelicEvent
{
    public RelicName relicName { get; private set; }


    public bool IsEventTrigger { get; private set; }

    private ISetPlayerSpecialAction setAction;

    public AllOverTheBurst(ISetPlayerSpecialAction _setAction, RelicName _relicName)
    {
        setAction = _setAction;
        relicName = _relicName;
    }

    public void RelicEventAction()
    {
        if (!IsEventTrigger)
        {
            setAction.SetSpecialAction(SpecialAction.AllOver);
            IsEventTrigger = true;
        }
    }

    public bool RelicEventTrigger()
    {
        return true;
    }
}

public class LethalEnd : IRelicEvent
{
    public RelicName relicName { get; private set; }


    public bool IsEventTrigger { get; private set; }
    public void RelicEventAction()
    {

    }

    public bool RelicEventTrigger()
    {
        return true;
    }
}
