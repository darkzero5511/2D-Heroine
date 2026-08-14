using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [Header("Stats")]
    public Stat maxHealth;
    public Stat_MajorGroup attribute;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;

    //[Header("Attribute")]
    public float GetMaxHealth()
    {
        float baseHp = maxHealth.GetValue();
        float bonusHp = attribute.vitality.GetValue() * 5;

        return baseHp + bonusHp;
    }
}
