public interface IUnitHackeable
{
    public bool Hacked { get; set; }

    public string ObjecktName { get; }

    public bool CanHacke { get; set; }
    void StatusDisp();
}
