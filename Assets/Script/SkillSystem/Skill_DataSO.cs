using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skil data - ")]
public class Skill_DataSO : ScriptableObject
{
    public int cost;

    public string displayName;
    [TextArea] public string description;
    public Sprite icon;
}
