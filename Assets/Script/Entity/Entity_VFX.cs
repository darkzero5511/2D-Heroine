using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;

    [Header("Element")]
    private Color originalHitVfxColors;

    [Header("Ice")]
    [SerializeField] private Color chillTarget = new Color32(179, 234, 255, 255);

    [Space]
    [SerializeField] private Color chillVfxColor = Color.white;
    [SerializeField] private GameObject chillVfxEffect;

    [Space]
    [Header("Fire")]
    [SerializeField] private Color burnTarget = new Color32(255, 125, 126, 255);

    [Space]
    [SerializeField] private Color fireVfxColor = Color.white;
    [SerializeField] private GameObject fireVfxEffect;

    [Space]
    [Header("Explosion")]
    [SerializeField] public GameObject explosionVfxEffect;

    [Space]
    [Header("Lightning")]
    [SerializeField] private Color shockTarget = new Color32(251, 241, 124, 255);

    [Space]
    [SerializeField] private Color lightningVfxColor = Color.white;
    [SerializeField] private GameObject lightningVfxEffect;

    [Space]
    [SerializeField] private Color shockVfxColor = Color.white;
    [SerializeField] public GameObject shockVfxEffect;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColors = sr.color;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCo(duration, chillTarget));

        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCo(duration, burnTarget));

        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCo(duration, shockTarget));
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timeHasPassed = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .8f;

        bool toggle = false;

        while (timeHasPassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit, ElementType element)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;

        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    public void UpdateOnHitElement(Transform target, ElementType elementType)
    {
        if (elementType == ElementType.None)
            hitVfxColor = originalHitVfxColors;

        if (elementType == ElementType.Ice)
            CreateOnHitElement(target, chillVfxEffect, chillVfxColor);

        if (elementType == ElementType.Fire)
            CreateOnHitElement(target, fireVfxEffect, fireVfxColor);

        if (elementType == ElementType.Lightning)
            CreateOnHitElement(target, lightningVfxEffect, lightningVfxColor);
    }

    private void CreateOnHitElement(Transform target, GameObject ElementVfxEffect, Color ElementColor)
    {
        GameObject vfx = Instantiate(ElementVfxEffect, target.position, Quaternion.identity);

        if (entity.facingDir == -1)
            vfx.transform.Rotate(0, 180, 0);
    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);

        StartCoroutine(OndamageVfxCo());
    }

    private IEnumerator OndamageVfxCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
