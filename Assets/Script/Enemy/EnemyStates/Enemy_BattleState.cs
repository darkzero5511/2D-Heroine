using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private float lastTimeWasInBattle;
    private bool alertMode;

    private Vector2 lastKnownPlayerPosition;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        if (player == null)
            player = enemy.GetPlayerReference();

        if (enemy.PlayerDetected())
        {
            UpdateBattleTimer();

            alertMode = false;
            lastKnownPlayerPosition = player.position;
        }
        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * enemy.activeSlowMultiplier * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.currentState == enemy.deathState)
            return;

        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();

            alertMode = false;
            lastKnownPlayerPosition = player.position;

            if (enemy.wallDetected)
            {
                StayAlert();
                return;
            }
        }

        if (!enemy.PlayerDetected())
        {
            if (enemy.wallDetected)
            {
                StayAlert();
                return;
            }

            if (!IsUnderPlayer() && !alertMode)
            {
                enemy.SetVelocity(enemy.GetBattleMoveSpeed() * 1.2f * DirectionToLastLocation(), rb.linearVelocity.y);
                if (DistanceToPlayer() < .2f)
                {
                    StayAlert();
                    return;
                }
            }
            else
            {
                StayAlert();
                return;
            }
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
            return;
        }
    }

    private void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;

        Transform newTarget = enemy.PlayerDetected().transform;

        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }

    private void UpdateBattleTimer()
        => lastTimeWasInBattle = Time.time;

    protected bool BattleTimeIsOver()
        => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;

    public bool WithinAttackRange()
        => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat()
        => DistanceToPlayer() < enemy.minRetreatDistance;

    public float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }

    private int DirectionToLastLocation()
    {
        if (player == null)
            return 0;

        return lastKnownPlayerPosition.x > enemy.transform.position.x ? 1 : -1;
    }

    private bool PlayerIsAbove()
    {
        if (player == null)
            return false;

        return player.position.y > enemy.transform.position.y + 0.5f;
    }

    private bool IsUnderPlayer()
    {
        if (player == null)
            return false;

        return Mathf.Abs(lastKnownPlayerPosition.x - enemy.transform.position.x) < 2f;
    }

    private void StayAlert()
    {
        enemy.HandleFlip(DirectionToPlayer());
        enemy.SetVelocity(0, rb.linearVelocity.y);

        alertMode = true;
        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);
    }
}
