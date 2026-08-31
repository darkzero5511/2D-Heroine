using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Stat Setup", fileName = "Default Stat Setup - ")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;
    public float maxMana = 100;
    public float manaRegen = .5f;

    [Header("Offense - Phyiscal Damage")]
    public float attackSpeed = 1;
    public float physicalDamage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorPenetration;

    [Header("Offense - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defense - Phyiscal Damage")]
    public float armor;
    public float evasion;

    [Header("Defense - Elemental Damage")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

    [Header("Status Effect")]
    public float chill = .2f;
    public float explosionDmg = .8f;
    public float explosionChance = .2f;
    public float electrify = .2f;

    [Header("Attribute Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
