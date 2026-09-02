using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_EquipSlot[] uiEquipSlots;

    [SerializeField] private UI_ItemSlotParent inventorySlotParents;
    [SerializeField] private Transform uiEquipSlotParents;

    private void Awake()
    {
        uiEquipSlots = uiEquipSlotParents.GetComponentsInChildren<UI_EquipSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotParents.UpdateSlots(inventory.itemList);
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlots> playerEquipList = inventory.equipList;

        for (int i = 0; i < uiEquipSlots.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if (playerEquipSlot.HasItem() == false)
                uiEquipSlots[i].UpdateSlot(null);
            else
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }
}
