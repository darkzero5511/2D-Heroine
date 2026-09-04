using System;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;

    [Header("Physical")]
    public Stat physicalDamage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorPenetration;

    [Space]
    [Header("Elemental")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
