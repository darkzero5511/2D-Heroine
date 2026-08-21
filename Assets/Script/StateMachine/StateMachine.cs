public class StateMachine
{
    public Entity_State currentState { get; private set; }
    public bool canChangeState = true;

    public void Initialize(Entity_State startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(Entity_State newState)
    {
        if (canChangeState == false)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOffStateMachine()
        => canChangeState = false;
}
