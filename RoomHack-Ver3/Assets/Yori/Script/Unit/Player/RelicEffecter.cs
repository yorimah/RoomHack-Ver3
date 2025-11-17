using System.Collections.Generic;
using UnityEngine;
public class RelicEffecter : MonoBehaviour
{
    List<IRelicEffecter> relicEffecters = new List<IRelicEffecter>();

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
}

public class DestroyerEffectBase : IRelicEffecter
{
    public string relicName { get; private set; }
    public bool canUse { get; private set; }
    public virtual void RelicEffect()
    {

    }
}