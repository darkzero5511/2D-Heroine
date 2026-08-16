using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [Header("Stats")]
    public Stat maxHealth;

    [Space]
    public Stat_MajorGroup attribute;

    [Space]
    public Stat_OffenseGroup offense;

    [Space]
    public Stat_DefenseGroup defense;

    //[Header("Attribute")]
    public float GetMaxHealth()
    {
        float baseHp = maxHealth.GetValue();
        float bonusHp = attribute.vitality.GetValue() * 5;

        return baseHp + bonusHp;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = attribute.agility.GetValue() * .5f;
        float totalEvasion = baseEvasion + bonusEvasion;

        float evasionCap = 20f; //Evasion will be capped at 20%

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }
}
