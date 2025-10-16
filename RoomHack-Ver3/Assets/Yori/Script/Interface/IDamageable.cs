public interface IDamageable
{
    public float maxHitPoint { get; }

    public float nowHitPoint { get; set; }

    /// <summary>
    /// 1  プレイヤー
    /// 2　敵
    /// 3　壁
    /// 4  爆発
    /// </summary>
    public int hitDamegeLayer { get; }

    public void HitDmg(int dmg, float hitStop)
    {
        nowHitPoint -= dmg;
        if (nowHitPoint <= 0)
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
