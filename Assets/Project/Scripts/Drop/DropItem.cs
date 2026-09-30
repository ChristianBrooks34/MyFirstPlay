using UnityEngine;

[System.Serializable]
public class DropItem
{
    [Range(0, 100)] public float DropRage;
    public int MaxDropCount;
    public int MinDropCount;
    public DropProfile DropProfile;
}
