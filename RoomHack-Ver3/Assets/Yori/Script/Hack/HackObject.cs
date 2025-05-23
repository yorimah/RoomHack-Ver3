using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HackObject 
{
    public int secLevele { set; get; }



    float HackSecond(int hackSpeedLevel)
    {
        float curve = 1.5f;
        float ratio = 3;
        float levelgap = secLevele - hackSpeedLevel;
        float hackSecond = Mathf.Pow(curve, levelgap) * ratio;
        return hackSecond;
    }

    float HackInpact(int hackDamage)
    {
        float intercept = 3;
        float levelGap = secLevele - hackDamage;
        float hackInpact = Mathf.Sqrt(levelGap + intercept);
        return Mathf.Abs(hackInpact);
    }
}
