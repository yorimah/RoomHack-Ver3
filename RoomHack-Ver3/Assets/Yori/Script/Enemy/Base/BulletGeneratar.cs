using UnityEngine;

public class BulletGeneratar : MonoBehaviour
{
    [SerializeField, Header("撃つ弾のプレハブ")]
    private GameObject bulletPrefab;
    public void GunFire(float bulletSpeed, int hitLayer, int power, float diffusionRate)
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();


        // 拡散の値決定

        float rand = Random.Range(-diffusionRate, diffusionRate);

        float playerMoveSpeed = UnitCore.Instance.moveSpeed;
        Vector3 playerMoveVector = UnitCore.Instance.GetComponent<Rigidbody2D>().velocity;
        Vector3 plNextPos = UnitCore.Instance.transform.position + playerMoveVector;



        Vector2 shotDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z + rand) * Vector3.up;

        bulletCore.HitDamegeLayer = hitLayer;
        bulletCore.hitStop = 0.1f;
        bulletCore.power = power;
        bulletRigit.velocity = shotDirection * bulletSpeed;
        bulletGameObject.transform.up = shotDirection;
    }
}
