using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/craftRecipe Data/Material craftRecipe", fileName = "Material Data - ")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("craftRecipe Effect")]
    [SerializeField] public ItemEffect_DataSO itemEffect;

    [Header("Craft details")]
    public Inventory_Item[] craftRecipe;
}
