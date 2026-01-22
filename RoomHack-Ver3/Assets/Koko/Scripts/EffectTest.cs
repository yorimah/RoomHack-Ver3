using UnityEngine;
using System.Collections.Generic;

using Zenject;

public class EffectTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = this.transform.position;
            //pos.z = -1;
            //Debug.Log(pos);
            EffectManager.Instance.ActEffect_Num(10, pos + Vector3.up, 2);
            EffectManager.Instance.ActEffect(EffectManager.EffectType.HitDamage, pos, this.transform.localEulerAngles.z, true);
        }
    }
}
