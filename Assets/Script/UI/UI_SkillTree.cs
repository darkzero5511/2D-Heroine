using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoint;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;

    public bool EnoughSkillPoints(int cost) => skillPoint >= cost;

    public void RemoveSkillPoint(int cost) => skillPoint -= cost;

    public void AddSkillPoints(int point) => skillPoint += point;

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
            if (node.isUnlocked)
                node.Refund();
    }

    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
