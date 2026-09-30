public enum UnitState
{
    Idle,
    Move,
    Attack,
    Dead
}

public class StateUnit
{
    public UnitState CurrentState { get; private set; }

    public void ChangeState(UnitState unitState)
    {
        CurrentState = unitState;
    }
}
