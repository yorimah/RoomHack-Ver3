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
    /// ���񂾂Ƃ��̏���
    /// </summary>
    public void Die();
}
