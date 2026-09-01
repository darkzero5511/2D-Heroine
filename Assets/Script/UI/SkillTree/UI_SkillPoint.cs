using TMPro;
using UnityEngine;

public class UI_SkillPoint : MonoBehaviour
{
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillPoint;

    private void Awake()
    {
        skillTree = GetComponentInParent<UI_SkillTree>();
        UpdateSkillPoint();
    }

    public void UpdateSkillPoint()
    {
        skillPoint.text = "Skill Point: " + skillTree.GetSkillPoint();
    }
}
