using UnityEngine;

public class AddTool : MonoBehaviour, ICardType
{
    
    [SerializeField, Header("追加するカードの種類")]
    private toolTag toolTag;
    PlayerSaveData data;
    public int cardWeight { get; set; } = 10;
    public int cardLevel { get; set; } = 1;

    public AddTool()
    {
        switch (toolTag)
        {
            case toolTag.none:
                break;
            case toolTag.CCTVHack:
                break;
            case toolTag.Bind:
                break;
            case toolTag.Blind:
                break;
            case toolTag.OverHeat:
                break;
            case toolTag.Detonation:
                cardLevel = 3;
                break;
        }
    }
    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;
        data.deckList.Add((int)toolTag);
        return data;
    }
}
