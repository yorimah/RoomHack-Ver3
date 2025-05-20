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
        // IDamegebable���^�����邩���ׂ�B�^������Ȃ�dmglayer�𒲂ׂē����邩���f
        if (collision.gameObject.TryGetComponent<IDamegeable>(out var damage))
        {
             Debug.Log("�U���ł���" + collision.gameObject.name + "�ɂ�������");
            if (this.HitDamegeLayer != damage.HitDamegeLayer)
            {
                 Debug.Log(collision.gameObject.name + "��" + 1 + "�̃_���[�W");
                damage.HitDmg(1);
            }
        }
    }


}
