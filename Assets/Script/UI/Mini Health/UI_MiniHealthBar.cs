using UnityEngine;
using UnityEngine.UI;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity
        => GetComponentInParent<Entity>();

    [SerializeField] private Slider slider;

    private CanvasGroup canvasGroup;
    private float lastValue;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        lastValue = slider.value;

        canvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        if (entity != null)
            entity.OnFlippped += HandleFlip;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.OnFlippped -= HandleFlip;
    }

    private void Update()
    {
        if (slider.value != lastValue)
        {
            lastValue = slider.value;
            canvasGroup.alpha = slider.value > 0 ? 1 : 0;
        }
    }

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
