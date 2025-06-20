using UnityEngine;

public class BulletGeneratar : MonoBehaviour
{
    [SerializeField, Header("撃つ弾のプレハブ")]
    private GameObject bulletPrefab;

    public void GunFire(float bulletSpeed,int hitLayer)
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        Vector2 shotDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;

        bulletCore.HitDamegeLayer = hitLayer;
        bulletCore.hitStop = 0.1f;
        bulletRigit.velocity = shotDirection * bulletSpeed;
        bulletGameObject.transform.up = shotDirection;
    }
}
