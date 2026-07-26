using UnityEngine;

public class Enity_AnimationTriggers : MonoBehaviour
{
    private Enity enity;

    private void Awake()
    {
        enity = GetComponentInParent<Player>();
    }

    public void CurrentStateTrigger()
    {
        enity.CallAnimationTrigger();
    }
}
