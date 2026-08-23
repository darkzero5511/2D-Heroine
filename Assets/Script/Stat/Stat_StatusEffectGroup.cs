using System;
using UnityEngine;

[Serializable]
public class Stat_StatusEffectGroup
{
    public float defaultDuration = 3;

    [Header("Status Effect connectionDetails")]
    public Stat chillSlowMultiplier;
    public Stat burnExplosion;
    public Stat explosionChance;
    public Stat electrifyChargeBuildUp;

    [Space]
    public float fireScale = .8f;
    public float lightningScale = 2.5f;
}
