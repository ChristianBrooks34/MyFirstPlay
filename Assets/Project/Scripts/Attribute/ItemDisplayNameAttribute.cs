
using System;

[AttributeUsage(AttributeTargets.Class)]
public class ItemDisplayNameAttribute : Attribute
{
    public string DisplayName { get; }
    public ItemDisplayNameAttribute(string displayName) => DisplayName = displayName;
}