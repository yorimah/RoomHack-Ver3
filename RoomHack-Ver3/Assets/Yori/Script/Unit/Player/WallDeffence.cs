using UnityEngine;

public class WallDeffence : MonoBehaviour, IDamageable
{

    public float maxHitPoint { get; set; }
    public float nowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 3;

    // Start is called before the first frame update
    void Start()
    {
        nowHitPoint = maxHitPoint;
    }

    public void HitDmg(int dmg,float hit)
    {

    }
    public void Die()
    {
        // 壁は死なない
    }
}
