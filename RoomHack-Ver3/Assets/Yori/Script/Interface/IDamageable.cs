public interface IDamageable
{
    public float MAXHP { get; }

    public float NowHP { get; set; }

    /// <summary>
    /// 1  プレイヤー
    /// 2　敵
    /// 3　壁
    /// 4  爆発
    /// </summary>
    public int HitDamegeLayer { get; set; }

    public void HitDmg(int dmg, float hitStop)
    {
        NowHP -= dmg;
        if (NowHP <= 0)
        {
            Die();
        }
        else
        {
            if (HitDamegeLayer == 2)
            {
                SeManager.Instance.Play("Hit");
            }
            HitStopper.Instance.StopTime(hitStop);
        }
    }
    /// <summary>
    /// 死んだときの処理
    /// </summary>
    public void Die();
}
