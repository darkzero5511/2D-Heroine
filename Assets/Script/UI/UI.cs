using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTree skillTreeUI;
    public UI_Inventory inventoryUI;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;

    private bool skillTreeEnabled;
    private bool inventoryEnabled;

    public void Awake()
    {
        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);

        skillToolTip = GetComponentInChildren<UI_SkillToolTip>(true);
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>(true);
        statToolTip = GetComponentInChildren<UI_StatToolTip>(true);

        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
    }

    public void ToggleInventoryUI()
    {
        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        skillToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }
}
