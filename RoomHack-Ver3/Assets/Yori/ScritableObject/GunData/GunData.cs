using UnityEngine;

[CreateAssetMenu(menuName = "GunData")]
public class GunData : ScriptableObject
{
    public int power;
    public int rate;
    public int MaxMagazine;
    public float reload;
    public float bulletSpeed;
    public float Maneuverability;
}
