using Unity.VisualScripting;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;
    private float investigateStartTime;

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
            lastKnownPlayerPosition = player.position;
        }
        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetected())
        {
            UpdateBattleTimer();
            lastKnownPlayerPosition = player.position;
        }

        if (!enemy.PlayerDetected() && enemy.wallDetected)
            StayAlert();

        if (!enemy.PlayerDetected() && PlayerIsAbove())
        {
            if (!IsUnderPlayer())
            {
                enemy.SetVelocity(enemy.battleMoveSpeed * 1.2f * DirectionToPlayer(), rb.linearVelocity.y);
                if (DistanceToPlayer() < .2f)
                    StayAlert();
            }
            else
                StayAlert();
            return;
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
    }

    private void UpdateBattleTimer()
        => lastTimeWasInBattle = Time.time;

    protected bool BattleTimeIsOver()
        => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;

    private bool WithinAttackRange()
        => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat()
        => DistanceToPlayer() < enemy.minRetreatDistance;

    private float DistanceToPlayer()
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

    private bool PlayerIsAbove()
    {
        if (player == null)
            return false;

        return player.position.y > enemy.transform.position.y + 0.5f;
    }

    private bool IsUnderPlayer()
    {
        //if (player == null)
        //    return false;

        return Mathf.Abs(lastKnownPlayerPosition.x - enemy.transform.position.x) < 2f;
    }

    private void StayAlert()
    {
        enemy.SetVelocity(0, rb.linearVelocity.y);
        Debug.Log("Stay Alert");
        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);
    }
}
