public interface IDamegeable
{
    public float MAXHP { get; }

    public float NowHP { get; set; }

    /// <summary>
    /// 1   �v���C���[
    /// �Q�@�G
    /// �R�@��
    /// </summary>
    public int HitDamegeLayer { get; set; }

    public void HitDmg(int dmg)
    {
        HitStopper.Instance.StopTime(0.1f);
        NowHP -= dmg;
        if (NowHP <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// ���񂾂Ƃ��̏���
    /// </summary>
    public void Die();
}
