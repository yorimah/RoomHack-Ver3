using UnityEngine;

public class BulletCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }

    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; }

    public int power;

    public float hitStop;

    public void Start()
    {
        MAXHP = 1;
        NowHP = MAXHP;
    }



    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<BulletCore>(out var bullet))
        {
            return;
        }
        // IDamegebableが与えられるか調べる。与えられるならdmglayerを調べて当たるか判断
        if (collision.gameObject.TryGetComponent<IDamegeable>(out var damage))
        {
            if (this.HitDamegeLayer != damage.HitDamegeLayer)
            {
                damage.HitDmg(power,hitStop);
                Die();
            }
        }
    }


}
