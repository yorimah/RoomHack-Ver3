using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deffence : MonoBehaviour,IDamegeable
{

    public float MAXHP { get; set; }
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 1;

    // Start is called before the first frame update
    void Start()
    {
        NowHP = MAXHP;
    }

    // Ž€
    public void Die()
    {
        
    }
}
