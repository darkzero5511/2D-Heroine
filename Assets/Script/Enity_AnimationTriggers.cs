using UnityEngine;

public class Enity_AnimationTriggers : MonoBehaviour
{
    private Enity enity;

    private void Awake()
    {
        enity = GetComponentInParent<Enity>();
    }

    public void CurrentStateTrigger()
    {
        enity.CurrentStateAnimationTrigger();
    }
}
