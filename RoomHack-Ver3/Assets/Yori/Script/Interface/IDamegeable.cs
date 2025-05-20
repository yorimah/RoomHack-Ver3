public interface IDamegeable
{
    public float MAXHP { get; }

    public float NowHP { get; set; }

    /// <summary>
    /// 1   ƒvƒŒƒCƒ„[
    /// ‚Q@“G
    /// ‚R@•Ç
    /// </summary>
    public int HitDamegeLayer { get; set; }

    public void HitDmg(int dmg)
    {
        NowHP -= dmg;
        if (NowHP <= 0)
        {
            Die();
        }
    }
    public void Die();
}
