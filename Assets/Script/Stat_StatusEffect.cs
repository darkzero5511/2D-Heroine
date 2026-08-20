using System;
using UnityEngine;

[Serializable]
public class Stat_StatusEffect
{
    public float defaultDuration = 3;

    [Header("Status Effect details")]
    public float chillSlowMultiplier = .2f;
    public float electrifyChargeBuildUp = .4f;

    [Space]
    public float fireScale = .8f;
    public float lightningScale = 2.5f;
}
