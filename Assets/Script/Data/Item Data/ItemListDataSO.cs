using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/craftRecipe Data/craftRecipe list", fileName = "List of items - ")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;
}
