public interface IDamageable
{
    public bool CanBeAttacked { get; set; }
    public Health Health { get; set; }
    public bool TryApplyDamage(float damage);
    public void ApplyDamage(float damage);
    public void Dead();
}
