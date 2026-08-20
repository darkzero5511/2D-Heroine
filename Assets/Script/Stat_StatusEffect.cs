using System;
using UnityEngine;

[Serializable]
public class Stat_StatusEffect
{
    public float defaultDuration = 3;

    [Header("Status Effect details")]
    public float chillSlowMultiplier = .2f;
    public float burnMultiplier = 0.7f;
}
