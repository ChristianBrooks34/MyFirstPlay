using UnityEngine;
using Zenject;

public class PlayerTriggerController : MonoBehaviour
{
    private PlayerEventManager _playerEventManager;

    [Inject]
    public void Construct(PlayerEventManager playerEventManager)
    {
        _playerEventManager = playerEventManager;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _playerEventManager.TriggerTriggerEnter2D(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        _playerEventManager.TriggerTriggerExit2D(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        _playerEventManager.TriggerTriggerStay2D(collider);
    }
}
