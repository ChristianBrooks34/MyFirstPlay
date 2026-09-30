using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Drop")]
public class DropProfile : BaseProfile<DropData>
{
    [SerializeField] private DropData _dropData;

    public GameObject DropPrefab;

    public override DropData BaseData
    {
        get
        {
            return _dropData;
        }
        set
        {
            if (value == null) return;
            _dropData = value;
        }
    }
}

[Serializable]
public class DropData : BaseData
{
    public int Count { get; set; }
}


