using System;
using UnityEngine;

public abstract class Drop : MonoBehaviour
{
    public bool IsCollected { get; set; }
    [NonSerialized] public DropProfile DropProfile;
}
