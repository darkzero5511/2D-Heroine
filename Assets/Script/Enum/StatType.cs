using UnityEngine;

public enum StatType
{
    //
    //Resource
    //
    MaxHealth,
    HealthRegen,
    MaxMana,
    ManaRegen,

    //
    //Attribute
    //
    Strength,
    Agility,
    Intelligence,
    Vitality,

    //
    //Offense
    //

    //Physical
    AttackSpeed,
    PhysicalDamage,
    CritChance,
    CritPower,
    ArmorPenetration,

    //Elemental
    FireDamage,
    IceDamage,
    LightningDamage,

    //
    //Defense
    //

    //Physical
    Armor,
    Evasion,

    //Elemental
    IceResistance,
    FireResistance,
    LightningResistance,

    //
    //Status
    //
    Chill,
    ExplosionDmg,
    ExplosionChance,
    Electrify
}
