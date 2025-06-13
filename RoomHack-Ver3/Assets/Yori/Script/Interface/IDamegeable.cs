public interface IDamegeable
{
    public float MAXHP { get; }

    public float NowHP { get; set; }

    /// <summary>
    /// 1   プレイヤー
    /// ２　敵
    /// ３　壁
    /// </summary>
    public int HitDamegeLayer { get; set; }

    public void HitDmg(int dmg,float hitStop)
    {
        HitStopper.Instance.StopTime(hitStop);
        NowHP -= dmg;
        if (NowHP <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// 死んだときの処理
    /// </summary>
    public void Die();
}
