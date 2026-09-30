using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [NonSerialized] public ProjectileTrigerChecker ProjectileTrigerChecker;

    public ProjectileProfile Profile;
    public Explosion Explosion;

    public Unit Target { get; private set; } // ? зачем тут таргет лучше сделать отдельный снаряд которому нужен таргеt
    public bool IsDestroyed { get; private set; }
    public bool IsExplosion { get; private set; }
    public abstract ProjectileContainer ProjectileContainer { get; protected set; }
    public abstract BaseProjectileMovement ProjectileMovement { get; protected set; }
    public Vector2 Direction { get; protected set; }

    public event Action OnInitialized;
    public event Action OnExplosion;

    private void OnDestroy()
    {
        IsDestroyed = true;
        Explosion.OnCanceled -= OnExplosionCanceled;
    }

    public virtual void Initialized(Vector2 direction, Unit sourceUnit = null)
    {
        if (sourceUnit != null) Profile.BaseData.SourceUnit = sourceUnit;
        Direction = direction;
        TriggerInitialize();
    }

    public void ActiveExplosive(GameObject directHitGameObject = null)
    {
        if (IsDestroyed || IsExplosion) return;

        if (IsDestroyed || gameObject == null || !gameObject.activeSelf)
            return;

        DisableComponents();

        Explosion.gameObject.SetActive(true);
        if (directHitGameObject != null && Profile.BaseData.SourceUnit.gameObject != directHitGameObject.transform.parent.gameObject)
        {
            Explosion.Explosive(this, directHitGameObject.transform.parent.gameObject);
        }
        else
        {
            Explosion.Explosive(this);
        }

        OnExplosion?.Invoke();

        IsExplosion = true;
    }

    public void SetTarget(Unit target)
    {
        Target = target;
    }

    protected void DisableComponents()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        var boxColider2D = gameObject.GetComponent<BoxCollider2D>();
        if (boxColider2D != null)
            boxColider2D.enabled = false;
    }

    private void Delete()
    {
        if (!IsDestroyed)
        {
            ProjectileContainer.Remove(ProjectileMovement);
            Destroy(gameObject);
        }
    }

    protected void OnExplosionCanceled()
    {
        Delete();
    }

    protected async UniTaskVoid TimeLiveCooldown()
    {
        float currentTime = 0f;

        while (currentTime < Profile.BaseData.TimeLiveInSeconds)
        {
            await UniTask.Delay(1000);

            currentTime += 1;
        }

        ActiveExplosive();
    }

    protected void TriggerInitialize()
    {
        OnInitialized?.Invoke();
    }
}
