using System;
using UnityEngine;

public static class TransformExtensions
{
    public static int GetHorizontalSign(this Transform transform)
    {
        return transform.localScale.x > 0 ? 1 : -1;
    }

    public static int GetVerticalSign(this Transform transform)
    {
        return transform.localScale.y > 0 ? 1 : -1;
    }
}

