public class Invincible : IEffect
{
    public bool IsActive { get; private set; }

    private Unit _unit;

    public Invincible(Unit unit)
    {
        _unit = unit;
    }

    public void Apply()
    {
        if (IsActive) return;
        IsActive = true;
        _unit.CanBeAttacked = false;
    }

    public void Remove()
    {
        _unit.CanBeAttacked = true;
        IsActive = false;
    }
}
