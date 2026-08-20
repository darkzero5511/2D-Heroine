using System.Collections;
using UnityEngine;

public class Entity_StatusHandle : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX vfx;
    private Entity_Stats stats;
    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reduceDuration = duration * (1 - iceResistance);

        StartCoroutine(ChilledEffectCo(reduceDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        //Apply VFX
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        vfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        //Stop VFX
        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
