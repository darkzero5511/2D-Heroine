using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnTeleport;
    private Skill_Shard shardManager;

    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private GameObject vfxTeleport;

    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed, Transform newTarget = null)
    {
        target = newTarget == null ? FindClosestTarget() : newTarget;
        this.speed = speed;
    }

    public void SetupShard(Skill_Shard shardManager)
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;

        float detonationTime = shardManager.GetDetonateTime();

        Invoke(nameof(Explode), detonationTime);
    }

    public void SetupShard(Skill_Shard shardManager, float detonationTime, bool canMove, float shardSpeed, Transform target = null)
    {
        this.shardManager = shardManager;
        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;

        Invoke(nameof(Explode), detonationTime);

        if (canMove)
            MoveTowardsClosestTarget(shardSpeed, target);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Explode();
    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);

        Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        //OnExplode?.Invoke();
        Destroy(gameObject);
    }

    public void Teleport()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(vfxTeleport, transform.position, Quaternion.identity);

        OnTeleport?.Invoke();
        Destroy(gameObject);
    }
}
