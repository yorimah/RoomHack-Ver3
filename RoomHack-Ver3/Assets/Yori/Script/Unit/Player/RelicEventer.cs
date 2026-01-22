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
    [Inject]
    IGetTime getTime;
    [Inject]
    IGetCleaFlag getCleaFlag;

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
                return new AllOverTheBurst(setPlayerSpecialAction, getTime, relicName);
            case RelicName.brawProtcol:
                return new BrawProtocal(playerScore, relicName, setMoveSpeed);
            case RelicName.lethalEnd:
                return new LethalEnd(getTime, useableRam, relicName);
            case RelicName.flameDesires:
                return new FlameDesires(getCleaFlag, setHitPoint, relicName);
            case RelicName.redemption:
                return new Redemption(setPlayerSpecialAction, getTime, relicName);
        }
        return new NoneRelic(); ;
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
    lethalEnd,
    flameDesires,
    redemption
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

    private IGetTime getTime;
    public AllOverTheBurst(ISetPlayerSpecialAction _setAction, IGetTime _getTime, RelicName _relicName)
    {
        setAction = _setAction;
        relicName = _relicName;
        getTime = _getTime;
        setAction.AddSpecialAction(SpecialAction.AllOver);
    }

    public void RelicEventAction()
    {
        if (!IsEventTrigger)
        {
            if (getTime.gameTime <= 1)
            {
                IsEventTrigger = true;
            }
        }
    }

    public bool RelicEventTrigger()
    {
        return true;
    }
}
public class Redemption : IRelicEvent
{
    public RelicName relicName { get; private set; }


    public bool IsEventTrigger { get; private set; }

    private ISetPlayerSpecialAction setAction;

    IGetTime getTime;
    public Redemption(ISetPlayerSpecialAction _setAction, IGetTime _getTime, RelicName _relicName)
    {
        setAction = _setAction;
        relicName = _relicName;
        getTime = _getTime;
        setAction.AddSpecialAction(SpecialAction.Redemption);
    }

    public void RelicEventAction()
    {
        if (!IsEventTrigger)
        {
            if (getTime.gameTime <= 0)
            {
                IsEventTrigger = true;
            }
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

    private IGetTime getTime;

    private int drawNum = 3;

    private IUseableRam useableRam;

    public LethalEnd(IGetTime _getTime, IUseableRam _useableRam, RelicName _relicName)
    {
        getTime = _getTime;
        useableRam = _useableRam;
        relicName = _relicName;
    }
    public void RelicEventAction()
    {
        if (RelicEventTrigger())
        {
            useableRam.RamChange(useableRam.RamCapacity);
            for (int i = 0; i < drawNum; i++)
            {
                ToolManager.Instance.DeckDraw();
            }
        }
    }

    public bool RelicEventTrigger()
    {
        if (!IsEventTrigger)
        {
            if (getTime.gameTime <= 1)
            {
                IsEventTrigger = true;
                return true;
            }
        }
        return false;
    }
}

public class FlameDesires : IRelicEvent
{
    public RelicName relicName { get; private set; }

    public bool IsEventTrigger { get; private set; }

    private IGetCleaFlag clearFlag;

    private ISetHitPoint setHitPoint;

    public FlameDesires(IGetCleaFlag _clearFlag, ISetHitPoint _setHitPoint, RelicName _relicName)
    {
        clearFlag = _clearFlag;
        relicName = _relicName;
        setHitPoint = _setHitPoint;
    }
    public void RelicEventAction()
    {
        if (RelicEventTrigger())
        {
            setHitPoint.HealNowHitPoint(30);
        }
    }
    public bool RelicEventTrigger()
    {
        if (clearFlag.isClear && !IsEventTrigger)
        {
            IsEventTrigger = true;
            return true;
        }
        return false;
    }
}
