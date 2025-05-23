using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : MonoBehaviour,HackObject,IDamegeable
{
    public int secLevele { get; set; }

    public float MAXHP { get; set; } = 10;
    public float NowHP { get; set; }
    public int HitDamegeLayer { get; set; } = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
