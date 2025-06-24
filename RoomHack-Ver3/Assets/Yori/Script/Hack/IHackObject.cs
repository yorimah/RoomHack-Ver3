public interface IHackObject
{
    public int secLevele { set; get; }

    public bool clacked { get; set; }

    public float MaxFireWall { get; set; }
    public float NowFireWall { get; set; }

    public float FireWallCapacity { get; set; }

    public void Clack(float BreachPower)
    {
        NowFireWall -= BreachPower;
        if (NowFireWall <= FireWallCapacity)
        {
            CapacityOver();
        }
    }

    public void CapacityOver();
}
