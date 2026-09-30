using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class TilemapInstaller : MonoInstaller
{
    [SerializeField] private Tilemap _greedTilemap;
    [SerializeField] private BuildingProfile _buildingProfile;
    [SerializeField] private Camera _camera;

    public override void InstallBindings()
    {
        Container
            .Bind<Tilemap>()
            .FromInstance(_greedTilemap)
            .AsSingle();

        Container
            .Bind<TileOccupancyMap>()
            .AsSingle();

        Container
            .BindInterfacesTo<TileMapClick>()
            .AsSingle()
            .WithArguments(_buildingProfile)
            .NonLazy();

        Container
            .Bind<CursorTileLocator>()
            .AsSingle()
            .WithArguments(_camera);
    }
}
