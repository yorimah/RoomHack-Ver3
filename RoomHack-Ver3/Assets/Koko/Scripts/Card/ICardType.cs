public interface ICardType
{
    public enum Card
    {
        MaxRum,
        RamRecover,
        HandSize,
        AddTool,
        RemoveTool,
        WeponChange,
        HitPointUP,
        MoveSpeedUp,
    }
    public int cardLevel { get; set; }

    public int cardWeight { get; set; }
    public PlayerSaveData Choiced(PlayerSaveData data);
}
