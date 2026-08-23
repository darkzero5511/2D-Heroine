using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private UI ui;
    private RectTransform rect;

    [SerializeField] private Skill_DataSO skillData;

    [Header("Skill Details")]
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;

    [SerializeField] private Color skillLockedColor = new Color32(145, 145, 145, 255);
    [SerializeField] private string lockedColorHex = "#919191";

    private Color lastColor;

    public bool isUnlocked;
    public bool isLocked;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();

        UpdateIconColor(skillLockedColor);
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }

    private bool CanbeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        return true;
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CanbeUnlocked())
            Unlock();
        else
            Debug.Log("Cant be unlocked");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, skillData);

        if (isUnlocked == false)
            UpdateIconColor(Color.white * 0.9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked == false)
            UpdateIconColor(lastColor);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
