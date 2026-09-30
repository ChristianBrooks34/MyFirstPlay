using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossPhase
{
    public int PhaseNumber;
    [Range(0, 1)] public float HealthThreshold;
    public List<BaseAttack> Attacks;
    public int AttackInterval;
}
