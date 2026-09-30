using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseProfile<TBaseData> : ScriptableObject where TBaseData : BaseData
{
    public abstract TBaseData BaseData { get; set; }

    public List<DevelopItemFloat> GetAllDevelops(BaseData baseData)
    {
        if (BaseData == null)
        {
            Debug.LogWarning($"{GetType().Name}: BaseData не инициализирован. ¬озвращаем пустой список.");
            return new List<DevelopItemFloat>();
        }

        var dataType = BaseData.GetType();

        return dataType
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            .Where(f => f.FieldType == typeof(DevelopItemFloat))
            .Select(field => field.GetValue(BaseData) as DevelopItemFloat)
            .Where(item => item != null)
            .ToList();
    }
}

