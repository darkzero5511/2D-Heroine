using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/craftRecipe Data/craftRecipe effect/Buff effect", fileName = "craftRecipe effect data - Buff")]
public class ItemEffect_Buff : ItemEffect_DataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    private Player_Stats playerStats;

    public override bool CanBeUsed(Player player)
    {
        if (player.stats.CanApplyBuffOf(source))
        {
            this.player = player;
            return true;
        }
        else
        {
            Debug.Log("Same buff effect cannot be applied twice!");
            return false;
        }
    }

    public override void ExecuteEffect()
    {
        player.stats.ApplyBuff(buffsToApply, duration, source);
        player = null;
    }
}
