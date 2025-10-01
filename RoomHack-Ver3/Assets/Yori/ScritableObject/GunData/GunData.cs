using UnityEngine;

[CreateAssetMenu(menuName = "GunData")]
public class GunData : ScriptableObject
{
    public int power;
    public int rate;
    public int MAXMAGAZINE;
    public float reloadTime;
    public float bulletSpeed;
    public float Maneuverability;
    public float recoil;
}
