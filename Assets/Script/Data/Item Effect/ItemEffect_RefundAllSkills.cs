using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/craftRecipe Data/craftRecipe effect/Refund all skills", fileName = "craftRecipe effect data - Refund all skills")]
public class ItemEffect_RefundAllSkills : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
