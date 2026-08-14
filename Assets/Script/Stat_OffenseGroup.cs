using System;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

[Serializable]
public class Stat_OffenseGroup
{
    [Header("Physical")]
    public Stat physicalDamage;
    public Stat critDamage;
    public Stat critChange;

    [Header("Elemental")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightDamage;
}
