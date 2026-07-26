using UnityEngine;

public class Enemy : Enity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;

    [Header("Movement Details")]
    public float idleTime = 2;

    [Space]
    public float movespeed = 1.4f;

    [Range(0, 2)] public float moveAnimSpeedMultiplier = 1;
}
