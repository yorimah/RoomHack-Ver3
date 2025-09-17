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
    GameObject healthBar;

    [SerializeField]
    GameObject player;

    private void Start()
    {
        maxHp = UnitCore.Instance.MAXHP;

    }

    private void Update()
    {
        this.transform.position = player.transform.position;

        nowHp = UnitCore.Instance.NowHP;

        Vector3 scale = healthBar.gameObject.transform.localScale;
        scale.x = nowHp / maxHp;
        healthBar.gameObject.transform.localScale = scale;

    }
}
