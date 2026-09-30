using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SpawnTurretAttack : SpawnBuildingAttack
{
    [Inject(Id = nameof(SpawnTurretAttack) + "parentForTurret")] private Transform _parentForTurret;
    [Inject(Id = nameof(SpawnTurretAttack) + "parentForTurretHealthBar")] private Transform _parentForTurretHealthBar;

    [SerializeField] protected TurretBuildingProfile buildingProfile;

    private PlayerEventManager _playerEventManager;

    private Player _player;
    private Building _building;
    private BuildingSpawnFactory _buildingSpawnFactory;
    private CursorTileLocator _cursorTileLocator;
    private BuildingPreviewSystem _buildingPreviewSystem;

    [Inject]
    public void Construct(PlayerEventManager playerEventManager, BuildingSpawnFactory buildingSpawnFactory, CursorTileLocator cursorTileLocator,
        Player player, BuildingPreviewSystem buildingPreviewSystem)
    {
        _player = player;
        _buildingSpawnFactory = buildingSpawnFactory;
        _playerEventManager = playerEventManager;
        _cursorTileLocator = cursorTileLocator;
        _buildingPreviewSystem = buildingPreviewSystem;

        _playerEventManager.OnBuildingAttackStart += Attack;
        _playerEventManager.OnStartedAttack += OnClickPlaceBuildingButton;
    }

    public override void Attack()
    {
        if (_player.CurrentAttack == this || CanAttack == false) return;
        if (_player.IsDead) return;

        _player.CurrentAttack = this;

        SpawnBuilding();
    }

    protected override void SpawnBuilding()
    {
        Vector3Int? tilePosition = _cursorTileLocator.GetTilePositionUnderCursor();

        if (!tilePosition.HasValue)
        {
            Debug.Log(" урсор не над тайлом Ч спавн отменЄн.");
            return;
        }

        Vector3Int resultPos = new Vector3Int(tilePosition.Value.x, tilePosition.Value.y, 0);

        _building = _buildingSpawnFactory.SpawnTurretBuilding(buildingProfile, resultPos,
            _parentForTurret, _parentForTurretHealthBar);

        if (_building == null)
        {
            Debug.LogWarning("Ќе получилось заспавнить здание");
        }
        else
        {
            _buildingPreviewSystem.StartPreview(_building);

            PlaceBuildingCoroutine().Forget();
        }

        _building.SourceUnit = _player;
    }

    protected async UniTaskVoid PlaceBuildingCoroutine()
    {
        if (_building.CurrentState == BuildingState.Completed)
        {
            _buildingPreviewSystem.StopPreview(_building);
            TriggerAttack();
            _player.CurrentAttack = null;
            return;
        }

        while (_building.CurrentState != BuildingState.Completed)
        {
            await UniTask.DelayFrame(2);
            _buildingPreviewSystem.UpdatePreviewPosition(_building);
        }

        _buildingPreviewSystem.StopPreview(_building);
        TriggerAttack();
        _player.CurrentAttack = null;
    }

    private void OnClickPlaceBuildingButton()
    {
        if (_building == null) return;

        Vector3Int? tilePosition = _cursorTileLocator.GetTilePositionUnderCursor();

        if (!tilePosition.HasValue)
        {
            Debug.Log(" урсор не над тайлом Ч спавн отменЄн.");
            return;
        }

        Vector3Int resultPos = new Vector3Int(tilePosition.Value.x, tilePosition.Value.y, 0);

        var isPlacement = _buildingSpawnFactory.ConfirmBuildingPlacement(_building, resultPos);

        if (!isPlacement) return;

        _building.CurrentState = BuildingState.Completed;

        _buildingPreviewSystem.StopPreview(_building);
    }
}
