using System;
using UnityEngine;

[Serializable]
public class Stat_DefenseGroup
{
    [Header("Physical Defense")]
    public Stat armor;
    public Stat evasion;

    [Header("Elemental Resistance")]
    public Stat fireRes;
    public Stat iceRes;
    public Stat lightRes;
}
