using System.Collections.Generic;

public enum UnitSize
{
    Small,
    Medium,
    Large,
    BossLarge
}

public class UnitSizeSettings
{
    public static readonly Dictionary<UnitSize, float> SizeScales = new()
    {
        [UnitSize.Small] = 0.5f,
        [UnitSize.Medium] = 1f,
        [UnitSize.Large] = 1.5f,
        [UnitSize.Large] = 2f
    };
}

