using UnityEngine;
using Zenject;

public class DamageBloodSplashEffectPool : BaseEffectPool
{
    public DamageBloodSplashEffectPool(GameObject prefab, int poolSize, DiContainer diContainer, Transform parent)
    {
        Prefab = prefab;
        Parent = parent;
        PoolSize = poolSize;
        DiContainer = diContainer;

        SpawnGameObjects();
    }
}
