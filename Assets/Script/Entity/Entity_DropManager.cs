using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemListDataSO[] dropData;

    [Header("Drop restrctions")]
    [SerializeField] private int maxRarityAmount = 1200;
    [SerializeField] private int maxItemsToDrop = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            DropItems();
    }

    public virtual void DropItems()
    {
        if (dropData == null)
        {
            Debug.Log("You need to assign drop data on entity" + gameObject.name);
            return;
        }

        List<ItemDataSO> itemsToDrop = RollDrops();
        int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

        for (int i = 0; i < amountToDrop; i++)
        {
            CreateItemDrop(itemsToDrop[i]);
        }
    }

    protected void CreateItemDrop(ItemDataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    public List<ItemDataSO> RollDrops()
    {
        List<ItemDataSO> possibleDrops = new List<ItemDataSO>();
        List<ItemDataSO> finalDrops = new List<ItemDataSO>();

        int remainingRarity = maxRarityAmount;

        // 1. Roll drop chance

        foreach (var dropList in dropData)
        {
            if (dropList == null || dropList.itemList == null)
                continue;

            foreach (var item in dropList.itemList)
            {
                if (item == null)
                    continue;

                float dropChance = item.GetDropChance();

                if (Random.Range(0f, 100f) < dropChance)
                    possibleDrops.Add(item);
            }
        }

        // 2. Sort highest rarity first

        possibleDrops = possibleDrops
            .OrderByDescending(item => item.itemRarity)
            .ToList();

        // 3. Select final drops

        foreach (var item in possibleDrops)
        {
            if (finalDrops.Count >= maxItemsToDrop)
                break;

            if (item.itemRarity > remainingRarity)
                continue;

            finalDrops.Add(item);
            remainingRarity -= item.itemRarity;
        }

        return finalDrops;
    }
}
