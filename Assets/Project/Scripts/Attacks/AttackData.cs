using UnityEngine;

[CreateAssetMenu(menuName = "Attack")]
public class AttackData : ScriptableObject
{
    public string Name;
    public float Damage;
    public float Cooldown;
    public float AttackRadius;
    public Sprite Icon;
}
