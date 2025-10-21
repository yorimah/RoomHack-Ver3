using UnityEngine;

public class WallDeffence : MonoBehaviour, IDamageable
{

    public float MaxHitPoint { get; set; }
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 3;

    // Start is called before the first frame update
    void Start()
    {
        NowHitPoint = MaxHitPoint;
    }

    public void HitDmg(int dmg,float hit)
    {

    }
    public void Die()
    {
        // 壁は死なない
    }
}
