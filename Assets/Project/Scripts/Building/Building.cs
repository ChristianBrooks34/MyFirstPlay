using UnityEngine;

public class Building : MonoBehaviour, ITargetInfo
{
    public BuildingProfile Profile;

    [SerializeField] private EntityType _type = EntityType.Building;
    public EntityType Type => _type;

    public BuildingState CurrentState { get; set; }
    public Unit SourceUnit { get; set; }

    public GameObject PlacementAllowedPanel;
    public GameObject CollisionFreePanel;
}
