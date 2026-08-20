using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [Header("Stats")]
    public Stat maxHealth;

    [Space]
    public Stat_AttributeGroup attribute;

    [Space]
    public Stat_OffenseGroup offense;

    [Space]
    public Stat_DefenseGroup defense;

    [Space]
    public Stat_StatusEffect statusEffect;

    //
    //STATS
    //

    public float GetElementalDamage(out ElementType element)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();

        float fireBonusDamage = attribute.intelligence.GetValue() * 1.2f;
        float iceBonusDamage = attribute.intelligence.GetValue() * 0.8f;
        float lightningBonusDamage = attribute.intelligence.GetValue() * 1f;

        float bonusElementalDamage = fireBonusDamage + iceBonusDamage + lightningBonusDamage;

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            element = ElementType.Ice;
            highestDamage = iceDamage;
        }

        if (lightningDamage > highestDamage)
        {
            element = ElementType.Lighting;
            highestDamage = lightningDamage;
        }

        if (highestDamage <= 0)
            return 0;

        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * .5f;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * .5f;
        float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage * .5f;

        float weakerElementDamage = bonusFire + bonusIce + bonusLightning;

        float finalDamage = highestDamage + bonusElementalDamage + weakerElementDamage;

        return finalDamage;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResitance = 0;
        float bonusResistance = attribute.intelligence.GetValue() * .5f;

        switch (element)
        {
            case ElementType.Fire:
                baseResitance = defense.fireRes.GetValue();
                break;

            case ElementType.Ice:
                baseResitance += defense.iceRes.GetValue();
                break;

            case ElementType.Lighting:
                baseResitance = defense.lightningRes.GetValue();
                break;
        }

        float resistance = baseResitance + bonusResistance;
        float mitigation = resistance / (resistance + 150);
        float mitigationCap = 50f;
        float finalResistance = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalResistance;
    }

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

    public float GetArmorMitigation(float armorReduction)
    {
        // Stat armor
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = attribute.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float reductionMutliper = Mathf.Clamp01(1 - armorReduction);
        float effectiveArmor = totalArmor * reductionMutliper;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = .85f; //Max mitigation will be capped at 85%
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorPenetration()
    {
        float finalPenetration = offense.armorPenetration.GetValue() / 100;

        return finalPenetration;
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

    public float GetMaxHealth()
    {
        float baseHp = maxHealth.GetValue();
        float bonusHp = attribute.vitality.GetValue() * 5;
        float finalHp = baseHp + bonusHp;

        return finalHp;
    }
}
