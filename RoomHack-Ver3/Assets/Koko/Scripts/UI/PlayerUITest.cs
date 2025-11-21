using UnityEngine;
using Zenject;
public class PlayerUITest : MonoBehaviour
{
    [SerializeField]
    float maxHp = 1;

    [SerializeField]
    float nowHp = 1;

    [SerializeField]
    GameObject hpBar;

    [SerializeField]
    float maxBullet = 1;

    [SerializeField]
    float nowBullet = 1;

    [SerializeField]
    GameObject bulletBar;

    [Inject]
    IGetHitPoint getHelth;
    [Inject]
    IPosition readPosition;

    [Inject]
    IHaveGun haveGun;

    [Inject]
    IGetGunData getGunData;
    private void Start()
    {
        maxHp = getHelth.MaxHitPoint;

        maxBullet = getGunData.GetGunData(haveGun.GunName).MaxBullet;
    }

    private void Update()
    {
        // Playerに追従
        this.transform.position = readPosition.PlayerPosition;

        // 現在HP取得
        nowHp = getHelth.NowHitPoint;

        // HPバーサイズ変更
        Vector3 hpScale = hpBar.gameObject.transform.localScale;
        hpScale.x = nowHp / maxHp;
        hpBar.gameObject.transform.localScale = hpScale;

        //現在残弾取得
        nowBullet = haveGun.BulletNow;

        // 残弾バーサイズ変更
        Vector3 bulletScale = bulletBar.gameObject.transform.localScale;
        bulletScale.x = nowBullet / maxBullet;
        bulletBar.gameObject.transform.localScale = bulletScale;

    }
}
