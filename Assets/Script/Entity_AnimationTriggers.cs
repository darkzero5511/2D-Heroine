using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity enity;
    private Entity_Combat enityCombat;

    private void Awake()
    {
        enity = GetComponentInParent<Entity>();
        enityCombat = GetComponentInParent<Entity_Combat>();
    }

    public void CurrentStateTrigger()
    {
        enity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        enityCombat.PerformAttack();
    }

    private void AttackTrigger3()
    {
        enityCombat.PerformAttack3();
    }
}
