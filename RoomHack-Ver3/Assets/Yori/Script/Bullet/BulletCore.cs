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
             Debug.Log("UŒ‚‚Å‚«‚é" + collision.gameObject.name + "‚É‚ ‚½‚Á‚½");
            if (this.HitDamegeLayer != damage.HitDamegeLayer)
            {
                 Debug.Log(collision.gameObject.name + "‚É" + 1 + "‚Ìƒ_ƒ[ƒW");
                damage.HitDmg(1);
            }
        }
    }


}
