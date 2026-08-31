using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material Data - ")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
}
