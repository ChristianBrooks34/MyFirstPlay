public interface IEffect
{
    void Apply();
    void Remove();
    bool IsActive { get; }
}
