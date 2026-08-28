using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;

    [SerializeField] private float coolDown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - coolDown;
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        coolDown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
            return false;

        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    private bool OnCooldown() => Time.time < lastTimeUsed + coolDown;

    public float SetSkillOnCooldonw() => lastTimeUsed = Time.time;

    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;

    public void ResetCooldown() => lastTimeUsed = Time.time;
}
