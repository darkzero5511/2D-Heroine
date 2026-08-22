using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;

    [Header("Stats")]
    public Stat_ResourceGroup resources;

    [Space]
    public Stat_OffenseGroup offense;

    [Space]
    public Stat_DefenseGroup defense;

    [Space]
    public Stat_StatusEffectGroup statusEffect;

    [Space]
    public Stat_AttributeGroup attribute;

    //
    //STATS
    //

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1f)
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
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = (element == ElementType.Fire) ? 0 : fireDamage * .5f;
        float bonusIce = (element == ElementType.Ice) ? 0 : iceDamage * .5f;
        float bonusLightning = (element == ElementType.Lighting) ? 0 : lightningDamage * .5f;

        float weakerElementDamage = bonusFire + bonusIce + bonusLightning;

        float finalDamage = highestDamage + bonusElementalDamage + weakerElementDamage;

        return finalDamage * scaleFactor;
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

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1f)
    {
        // Physical Damage
        float baseDamage = offense.physicalDamage.GetValue();
        float bonusDamage = attribute.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        //Crit Change
        float baseCritChange = offense.critChance.GetValue();
        float bonusCritChange = attribute.agility.GetValue() * .3f;
        float critChange = baseCritChange + bonusCritChange;

        //Crit Dmg
        float baseCritDamage = offense.critPower.GetValue();
        float bonusCritDamage = attribute.strength.GetValue() * .5f;
        float critDamage = (baseCritDamage + bonusCritDamage) / 100;

        // Crit Check
        isCrit = Random.Range(0, 100) < critChange;
        float finalDamage = isCrit ? totalBaseDamage * critDamage : totalBaseDamage;

        return finalDamage * scaleFactor;
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
        float baseHp = resources.maxHealth.GetValue();
        float bonusHp = attribute.vitality.GetValue() * 5;
        float finalHp = baseHp + bonusHp;

        return finalHp;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            //Resource
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;
            case StatType.MaxMana: return resources.maxMana;
            case StatType.ManaRegen: return resources.manaRegen;

            //Attribute
            case StatType.Strength: return attribute.strength;
            case StatType.Agility: return attribute.agility;
            case StatType.Intelligence: return attribute.intelligence;
            case StatType.Vitality: return attribute.vitality;

            //Offense
            //Physical
            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.PhysicalDamage: return offense.physicalDamage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorPenetration: return offense.armorPenetration;

            //Elemental
            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            //Defense
            //Physical
            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            //Elemental
            case StatType.IceResistance: return defense.iceRes;
            case StatType.FireResistance: return defense.fireRes;
            case StatType.LightningResistance: return defense.lightningRes;

            //Status
            case StatType.Chill: return statusEffect.chillSlowMultiplier;
            case StatType.ExplosionDmg: return statusEffect.burnExplosion;
            case StatType.ExplosionChance: return statusEffect.explosionChance;
            case StatType.Electrify: return statusEffect.electrifyChargeBuildUp;

            default:
                Debug.LogWarning($"StatType {type} not implemented yet.");
                return null;
        }
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);
        resources.maxMana.SetBaseValue(defaultStatSetup.maxMana);
        resources.manaRegen.SetBaseValue(defaultStatSetup.manaRegen);

        attribute.strength.SetBaseValue(defaultStatSetup.strength);
        attribute.agility.SetBaseValue(defaultStatSetup.agility);
        attribute.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        attribute.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.physicalDamage.SetBaseValue(defaultStatSetup.physicalDamage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorPenetration.SetBaseValue(defaultStatSetup.armorPenetration);

        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);

        statusEffect.chillSlowMultiplier.SetBaseValue(defaultStatSetup.chill);
        statusEffect.burnExplosion.SetBaseValue(defaultStatSetup.explosionDmg);
        statusEffect.explosionChance.SetBaseValue(defaultStatSetup.explosionChance);
        statusEffect.electrifyChargeBuildUp.SetBaseValue(defaultStatSetup.electrify);
    }
}
