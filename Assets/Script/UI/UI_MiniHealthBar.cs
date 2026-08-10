using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity
        => GetComponentInParent<Entity>();

    private void OnEnable()
    {
        entity.OnFlippped += HandleFlip;
    }

    private void OnDisable()
    {
        entity.OnFlippped -= HandleFlip;
    }

    private void HandleFlip()
        => transform.rotation = Quaternion.identity;
}
