using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Unit/Enemy")]
public class EnemyProfile : UnitProfile
{
    [SerializeField] private EnemyData _enemyData;

    private void OnEnable()
    {
        BaseData = Data;
    }

    public override void InitializeUnitData()
    {
        if (BaseData != null)
        {
            //Debug.Log($"{name}: Данные уже инициализированы. Пропускаем.");
            return;
        }

        BaseData = Data;
        //Debug.Log($"{name}: Инициализирован {Data.Name} по умолчанию.");
    }

    public EnemyData Data
    {
        get
        {
            return _enemyData;
        }
        set
        {
            if (value == null) return;
            _enemyData = value;
            BaseData = value;
        }
    }
}

[Serializable]
public class EnemyData : UnitData
{

}

