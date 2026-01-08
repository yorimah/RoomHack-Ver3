using UnityEngine;

public class BombBlast : MonoBehaviour
{
    [SerializeField, Header("爆発半径")]
    public float explosionRadial;
    [SerializeField, Header("爆発威力")]
    public int explosionPower;
    public int hitDamegeLayer { get; set; } = 4;
    private CircleCollider2D circleCollider2D;

    public bool isExplosion = false;
    public void Start()
    {
        Destroy(gameObject, 0.5f);
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        circleCollider2D.radius = explosionRadial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExplosion)
        {
            // IDamegebableが与えられるか調べる。与えられるならdmglayerを調べて当たるか判断
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damage))
            {
                if (hitDamegeLayer != damage.hitDamegeLayer)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position - transform.position);
                    ///Debug.DrawRay(transform.position, collision.transform.position - transform.position);
                    if (hit.collider.gameObject == collision.gameObject)
                    {
                        damage.HitDmg(explosionPower, 0);
                        //EffectManager.Instance.ActEffect_Num(explosionPower, this.transform.position, 1f);
                    }
                }
            }
        }
    }
}
