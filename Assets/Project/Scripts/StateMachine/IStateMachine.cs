public interface IStateMachine
{
    void ChangeState(object state);
    object CurrentState { get; }
    bool IsCurrentState(object state);
}
