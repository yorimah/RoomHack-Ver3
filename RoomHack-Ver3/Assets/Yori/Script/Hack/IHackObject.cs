using System.Collections.Generic;

public interface IHackObject
{
    public List<toolTag> canHackToolTag { get; set; }

    public List<ToolEvent> nowHackEvent { get; set; }

    //public void Hacking(toolTag _toolTag);


    // 以下初期制作、RIPよりまー

    //public int secLevele { set; get; }

    //public bool clacked { get; set; }

    //public float MaxFireWall { get; set; }
    //public float NowFireWall { get; set; }

    //public float FireWallCapacity { get; set; }

    //public float FireWallRecovaryNum { get; set; }

    //public void Clack(float BreachPower)
    //{
    //    NowFireWall -= BreachPower ;
    //    if (NowFireWall < FireWallCapacity)
    //    {
    //        CapacityOver();
    //    }
    //}

    //public void CapacityOver();

    //public void FireWallRecavary();
}
