using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class RelicEffecter : MonoBehaviour
{
    [SerializeField]
    List<IRelicEffecter> relicEffecters = new List<IRelicEffecter>();
    [Inject]
    IGetPlayerScore getScore;

    public void Start()
    {
        DestroyerEffectBase destroyer = new(getScore);
        relicEffecters.Add(destroyer);
    }
    private void Update()
    {
        foreach (var item in relicEffecters)
        {
            item.RelicEffect();
        }
    }
}

public interface IRelicEffecter
{
    public string relicName { get; }
    public bool canUse { get; }
    public void RelicEffect();

    public bool RelicEffectTrriger();
}

public class DestroyerEffectBase : IRelicEffecter
{
    IGetPlayerScore getScore;
    int nowScore;
    public DestroyerEffectBase(IGetPlayerScore _getScore)
    {
        getScore = _getScore;
    }
    public string relicName { get; private set; }
    public bool canUse { get; private set; }
    public virtual void RelicEffect()
    {
        if (RelicEffectTrriger())
        {

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