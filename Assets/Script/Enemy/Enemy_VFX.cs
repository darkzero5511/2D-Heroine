using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("Counter Attack Window")]
    [SerializeField] private GameObject attackAlert;
    [SerializeField] private bool alertMode = true;

    public void EnableAttackAlert(bool enable)
    {
        if (attackAlert == null || alertMode == false)
            return;

        attackAlert.SetActive(enable);
    }
}
