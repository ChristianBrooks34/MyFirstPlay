using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class WeaponProfile : ScriptableObject
{
    public string Name;
    public float Gamage;
    public SpriteRenderer Sprite;
    public GameObject Tamplate;
}
