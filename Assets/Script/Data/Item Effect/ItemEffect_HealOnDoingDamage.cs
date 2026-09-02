using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/craftRecipe Data/craftRecipe effect/Heal on doing damage", fileName = "craftRecipe effect data - Heal on doing phys damage")]

public class ItemEffect_HealOnDoingDamage : ItemEffect_DataSO
{
    [SerializeField] private float percentHealedOnAttack = .2f;


    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * percentHealedOnAttack);
    }
}
