using UnityEngine;
using System.Collections.Generic;
using System;

public enum GunName
{
    HandGun,
    AssuleRifle,
    SniperRifle,
    SubMachineGun,
    ShotGun,
    Revolver
}
[CreateAssetMenu(menuName = "GunData")]
public class GunData : ScriptableObject
{
    [SerializeField] private GunName gunName;
    [SerializeField] private int power;
    [SerializeField] private int rate;
    [SerializeField] private int maxBullet;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool reloadTypeBolt;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float maneuverability;
    [SerializeField] private float recoil;
    [SerializeField] private float minDiffusionRate;
    [SerializeField] private float maxDiffusionRate;
    [SerializeField] private int shotBulletNum;
    public GunName GunName { get => gunName; }
    public int Power { get => power; }
    public int Rate { get => rate; }
    public int MaxBullet { get => maxBullet; }
    public float ReloadTime { get => reloadTime; }
    public bool ReloadTypeBolt { get => reloadTypeBolt; }
    public float BulletSpeed { get => bulletSpeed; }
    public float Maneuverability { get => maneuverability; }
    public float Recoil { get => recoil; }
    public float MinDiffusionRate { get => minDiffusionRate; }
    public float MaxDiffusionRate { get => maxDiffusionRate; }
    public float ShotIntervalTime { get => 1f / rate; }
    public int ShotBulletNum { get => shotBulletNum; }
}