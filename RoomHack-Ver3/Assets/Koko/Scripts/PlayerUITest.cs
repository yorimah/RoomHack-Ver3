using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField, Tooltip("Playerオブジェクトアタッチ必須")]
    GameObject player;

    private void Start()
    {
        maxHp = UnitCore.Instance.MAXHP;

    }

    private void Update()
    {
        // Playerに追従
        this.transform.position = player.transform.position;

        // 現在HP取得
        nowHp = UnitCore.Instance.NowHP;

        // HPバーサイズ変更
        Vector3 hpScale = hpBar.gameObject.transform.localScale;
        hpScale.x = nowHp / maxHp;
        hpBar.gameObject.transform.localScale = hpScale;

        // 残弾バーサイズ変更
        Vector3 bulletScale = hpBar.gameObject.transform.localScale;
        bulletScale.x = nowHp / maxHp;
        hpBar.gameObject.transform.localScale = bulletScale;

    }
}
