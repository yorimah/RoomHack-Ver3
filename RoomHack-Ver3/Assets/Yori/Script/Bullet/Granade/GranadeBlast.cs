using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeBlast : MonoBehaviour
{
    public int HitDamegeLayer { get; set; } = 4;
    public void Die()
    {
        Destroy(gameObject);
    }
}
