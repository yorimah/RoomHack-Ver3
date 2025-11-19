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
                return new HitPointHeal(getScore, relicName);
            case RelicName.destoryRamHeal:
                return new RamHeal(getScore, relicName);
        }
        return null;
    }
    private void Update()
    {
        if (relicEffecters != null)
        {
            foreach (var item in relicEffecters)
            {
                if (item.RelicEffectTrriger())
                {
                    item.RelicEffect();
                }
            }
        }
    }
}
public enum RelicName
{
    destoryHPHeal,
    destoryRamHeal,
}
public class RelicGenarater
{

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
    public DestroyerEffectBase(IGetPlayerScore _getScore, RelicName _relicName)
    {
        getScore = _getScore;
        nowScore = getScore.GetDestroyScore();
        relicName = _relicName;
    }
    IRelicStatusEffect relicStatusEffect;
    public bool canUse { get; private set; }
    public virtual void RelicEffect()
    {
        Debug.Log(relicName + " : が起動したよ");
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
    public HitPointHeal(IGetPlayerScore _getScore, RelicName _relicName) : base(_getScore, _relicName)
    {
        _relicName = RelicName.destoryHPHeal;
    }

    public override void RelicEffect()
    {
        Debug.Log("HP回復！");
    }
}

public class RamHeal : DestroyerEffectBase
{
    public RamHeal(IGetPlayerScore _getScore, RelicName _relicName) : base(_getScore, _relicName)
    {
        _relicName = RelicName.destoryHPHeal;
    }

    public override void RelicEffect()
    {
        Debug.Log("Ram回復！");
    }
}

