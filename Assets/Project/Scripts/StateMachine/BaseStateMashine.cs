using System;

public abstract class BaseStateMashine<T> where T : Enum
{
    public abstract T CurrentState { get; protected set; }
}
