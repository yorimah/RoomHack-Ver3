using UnityEngine;

public class BulletGeneratar : MonoBehaviour
{
    [SerializeField, Header("撃つ弾のプレハブ")]
    private GameObject bulletPrefab;
    public void GunFire(int HitDamageLayer, Vector2 shotDirecition, float bulletSpeed)
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        bulletCore.HitDamegeLayer = HitDamageLayer;
        bulletCore.power = 40;
        bulletCore.hitStop = 0.1f;
        bulletRigit.velocity = shotDirecition * bulletSpeed;
        bulletGameObject.transform.up = shotDirecition;
    }
}
