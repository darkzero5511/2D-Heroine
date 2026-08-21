using System.Collections;
using UnityEngine;

public class Entity_StatusHandle : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;
    private ElementType currentEffect = ElementType.None;

    [Header("Electrify Effect Details")]
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
    }

    //
    //DoLightningStrike
    //

    public void ApplyElectrifyEffect(float duration, float lightningDamage, float charge)
    {
        float lightningResistance = entityStats.GetElementalResistance(ElementType.Lighting);
        float finalCharge = charge * (1 - lightningResistance);

        currentCharge += finalCharge;

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(lightningDamage);
            StopElectrifyEffect();
            return;
        }

        if (electrifyCo != null)
            StopCoroutine(electrifyCo);

        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllVfx();
    }

    private void DoLightningStrike(float lightningDamage)
    {
        Instantiate(entityVfx.electrifyVfxEffect, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(lightningDamage);
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lighting;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lighting);

        yield return new WaitForSeconds(duration);

        StopElectrifyEffect();
    }

    //
    // FIRE
    //

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = entityStats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnEffectCo(duration, fireDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int tickPerSecod = 2;
        int tickCount = Mathf.RoundToInt(tickPerSecod * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / tickPerSecod;

        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        //Stop VFX
        currentEffect = ElementType.None;
    }

    //
    // Explosion
    //
    public void ApplyExplosionEffect(float fireDamage)
    {
        Instantiate(entityVfx.explosionVfxEffect, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(fireDamage);
    }

    //
    //Ice
    //

    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance);

        StartCoroutine(ChilledEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        //Apply VFX
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        //Stop VFX
        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lighting && currentEffect == ElementType.Lighting)
            return true;

        return currentEffect == ElementType.None;
    }
}
