using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageRules", menuName = "Game/Damage Rules")]
public class DamageRules : ScriptableObject
{
    [System.Serializable]
    public class AttackRule
    {
        public EntityType attackerType;
        public List<EntityType> allowedTargets = new List<EntityType>();
    }

    [Tooltip("Здесь ты задаёшь все правила: кто кого может бить")]
    public List<AttackRule> rules = new List<AttackRule>();

    /// <summary>
    /// Возвращает true, если атакующий может бить цель.
    /// Вызывается в момент нанесения урона.
    /// </summary>
    public bool CanDamage(EntityType attacker, EntityType target)
    {
        var rule = rules.Find(r => r.attackerType == attacker);
        if (rule == null) return false;

        return rule.allowedTargets.Contains(target);
    }
}


