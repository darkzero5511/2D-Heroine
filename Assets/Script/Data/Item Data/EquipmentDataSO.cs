using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/craftRecipe Data/Equipment craftRecipe", fileName = "Equipment Data - ")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("craftRecipe Modifiers")]
    public ItemModifier[] modifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}
