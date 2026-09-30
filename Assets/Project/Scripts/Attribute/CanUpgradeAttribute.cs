using System;

[AttributeUsage(AttributeTargets.Field)]
public class CanUpgradeAttribute : Attribute
{
    public bool CanUpgrade { get; }

    public CanUpgradeAttribute(bool canUpgrade = true)
    {
        CanUpgrade = canUpgrade;
    }
}
