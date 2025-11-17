using UnityEngine;

public class EffectTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EffectManager.Instance.ActEffect(EffectManager.EffectType.HitDamage, this.transform.position, this.transform.localEulerAngles.z, true);



        }
    }
}
