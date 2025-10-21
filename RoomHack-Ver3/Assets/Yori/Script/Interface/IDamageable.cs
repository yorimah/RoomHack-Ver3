public interface IDamageable
{
    public float MaxHitPoint { get; }

    // 頑張って読み取り
    public float NowHitPoint { get; set; }

    /// <summary>
    /// 1  プレイヤー
    /// 2　敵
    /// 3　壁
    /// 4  無差別
    /// </summary>
    public int hitDamegeLayer { get; }

    public void HitDmg(int dmg, float hitStop)
    {
        NowHitPoint -= dmg;
        if (NowHitPoint <= 0)
        {
            Die();
        }
        else
        {
            if (hitDamegeLayer == 2)
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
