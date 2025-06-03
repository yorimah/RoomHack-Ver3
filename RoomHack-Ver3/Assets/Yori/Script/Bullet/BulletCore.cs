using UnityEngine;

public class BulletCore : MonoBehaviour, IDamegeable
{
    public float MAXHP { get; set; }

    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; }

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
        // IDamegebable‚ª—^‚¦‚ç‚ê‚é‚©’²‚×‚éB—^‚¦‚ç‚ê‚é‚È‚çdmglayer‚ğ’²‚×‚Ä“–‚½‚é‚©”»’f
        if (collision.gameObject.TryGetComponent<IDamegeable>(out var damage))
        {
            if (this.HitDamegeLayer != damage.HitDamegeLayer)
            {
                damage.HitDmg(1);
                Die();
            }
        }
    }


}
