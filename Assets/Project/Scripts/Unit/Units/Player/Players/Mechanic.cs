using System.Collections.Generic;
using Zenject;

public class Mechanic : Player
{
    [Inject]
    public void Construct(PlayerEventManager playerEventManager, List<BaseAttack> baseAttacks,
        LevelEventManager levelEventManager)
    {
        BaseAttacks = baseAttacks;
        this.playerEventManager = playerEventManager;
        this.levelEventManager = levelEventManager;

        playerEventManager.OnPickableEnter += TryPick;
    }
}
