public class PlayerData
{
    // プレイヤー初期値
    public float moveSpeed;
    public float maxHitPoint;
    public float nowHitPoint;
    public int hitDamegeLayer;
    // ハックデータ
    public float ramCapacity;
    public float nowRam;
    public float ramRecovary;
    // Ram回復系 
    public bool isRebooting = false;
    public float rebootTimer { get; private set; } = 0;
    public  PlayerData(PlayerSaveData saveData)
    {
        moveSpeed = saveData.moveSpeed;

        maxHitPoint = saveData.maxHitPoint;
        nowHitPoint = maxHitPoint;

        ramCapacity = saveData.maxRamCapacity;
        nowRam = ramCapacity;
        ramRecovary = saveData.RamRecovery;
    }
}
