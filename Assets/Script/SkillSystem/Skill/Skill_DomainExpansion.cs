using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Domain details")]
    public float maxDomainSize = 4.5f;
    public float expandSpeed = 3;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowPercent = .8f;
    [SerializeField] private float slowDuration = 5;

    [Header("Shard Casting Upgrade")]
    [SerializeField] private int shardToCast = 10;
    [SerializeField] private float shardSlowDuration = 1;
    [SerializeField] private float shardDuration = 8;

    [Header("Time Echo Casting Upgrade")]
    [SerializeField] private int echoToCast = 10;
    [SerializeField] private float echoSlowDuration = 1;
    [SerializeField] private float echoDuration = 8;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;
    private float spellCastTimer;
    private float spellsPerSecond;

    public void CreateDomain()
    {
        spellsPerSecond = GetSpellsToCast() / GetDomainDuration();

        if (upgradeType == SkillUpgradeType.Domain_SlowingDown)
            player.vFX.DoImageEchoEffect(skillManager.domainExpansion.GetDomainDuration());

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindTargetInDomain();

        if (currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1 / spellsPerSecond;
            currentTarget = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < .5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }

        if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);

        if (trappedTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }

    public float GetDomainDuration()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDuration;
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardDuration;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoDuration;

        return 0;
    }

    public float GetSlowPercentage()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDuration;
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardSlowDuration;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoSlowDuration;

        return 0;
    }

    private int GetSpellsToCast()
    {
        if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardToCast;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoToCast;

        return 0;
    }

    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_EchoSpam
            && upgradeType != SkillUpgradeType.Domain_ShardSpam;
    }

    public void AddTarget(Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);
    }

    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
            enemy.StopSlowDown();

        trappedTargets = new List<Enemy>();
    }
}
