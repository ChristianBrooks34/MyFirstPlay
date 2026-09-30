using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float LerpSpeed;

    private Transform _target;
    private SpawnUnitEventManager _spawnUnitEventManager;

    [Inject]
    public void Construct(SpawnUnitEventManager spawnUnitEventManager)
    {
        _spawnUnitEventManager = spawnUnitEventManager;

        _spawnUnitEventManager.OnPlayerSpawn += Initialize;
    }

    private void Initialize(Player player)
    {
        _target = player.transform;
    }

    public void Update()
    {
        if (_target == null) return;
        if (_camera.transform.position == _target.position) return;

        _camera.transform.position =
            Vector3.Lerp(_camera.transform.position, GetMoveOffset(), LerpSpeed * Time.deltaTime);
    }

    private Vector3 GetMoveOffset()
    {
        return new Vector3(
            _target.position.x,
            _target.position.y,
            _camera.transform.position.z);
    }

}
