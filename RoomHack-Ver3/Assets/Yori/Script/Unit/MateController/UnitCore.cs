using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCore : MonoBehaviour,IDamegeable
{
    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 2;

    void Start()
    {
        MAXHP = 10;
        NowHP = MAXHP;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Die()
    {

    }
}
