using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/craftRecipe Data/craftRecipe effect/Grand skill point", fileName = "craftRecipe effect data - Grant Skill Point")]

public class ItemEffect_GrantSkillPoint : ItemEffect_DataSO
{
    [SerializeField] private int pointsToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointsToAdd);
    }
}
