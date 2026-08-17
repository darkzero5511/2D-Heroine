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

    //
    //STATS
    //

    public float GetPhysicalDamage(out bool isCrit)
    {
        // Physical Damage
        float baseDamage = offense.physicalDamage.GetValue();
        float bonusDamage = attribute.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        //Crit Change
        float baseCritChange = offense.critChange.GetValue();
        float bonusCritChange = attribute.agility.GetValue() * .3f;
        float critChange = baseCritChange + bonusCritChange;

        //Crit Dmg
        float baseCritDamage = offense.critDamage.GetValue();
        float bonusCritDamage = attribute.strength.GetValue() * .5f;
        float critDamage = (baseCritDamage + bonusCritDamage) / 100;

        // Crit Check
        isCrit = Random.Range(0, 100) < critChange;
        float finalDamage = isCrit ? totalBaseDamage * critDamage : totalBaseDamage;

        return finalDamage;
    }

    public float GetMaxHealth()
    {
        float baseHp = maxHealth.GetValue();
        float bonusHp = attribute.vitality.GetValue() * 5;
        float finalHp = baseHp + bonusHp;

        return finalHp;
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
