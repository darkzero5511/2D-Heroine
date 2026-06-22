using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;

    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        anim = player.anim;
        rb = player.rb;
    }

    public virtual void Enter()
    {
        // evertime state will be chaned, enter will be called
        anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        //we going to run logic of the state here
        Debug.Log(animBoolName + " State");
    }

    public virtual void Exit()
    {
        // this will be called, every time we exit state and change to a new one
        anim.SetBool(animBoolName, false);
    }
}