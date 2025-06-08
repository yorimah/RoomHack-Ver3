using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHackObject 
{
    public int secLevele { set; get; }

    public bool hacked { get; set; }

    public void HackStart(int hackSpeedLevel, int hackDamage);

}
