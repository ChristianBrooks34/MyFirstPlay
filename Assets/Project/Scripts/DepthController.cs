using UnityEngine;
using Zenject;

public class DepthController : ITickable
{
    private Unit _unit;
    private SpriteRenderer _spriteRenderer;

    public DepthController(Unit unit)
    {
        _unit = unit;
    }

    public void Initialize()
    {
        _spriteRenderer = _unit.GetComponent<SpriteRenderer>();
    }

    public void Tick()
    {
        if (_spriteRenderer == null || _unit.IsDead) return;

        _spriteRenderer.sortingOrder = (int)(-_unit.transform.position.y);
    }
}
