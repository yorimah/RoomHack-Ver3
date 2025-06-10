using UnityEngine;

public class WallDeffence : MonoBehaviour, IDamegeable
{

    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 3;

    // Start is called before the first frame update
    void Start()
    {
        NowHP = MAXHP;
    }

    public void HitDmg(int dmg,float hit)
    {

    }
    public void Die()
    {
        // •Ç‚ÍŽ€‚È‚È‚¢
    }
}
