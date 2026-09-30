using UnityEngine;
using Zenject;

public class DeadBloodSplashEffectPool : BaseEffectPool
{
    public DeadBloodSplashEffectPool(GameObject prefab, int poolSize, DiContainer diContainer, Transform parent)
    {
        Prefab = prefab;
        Parent = parent;
        PoolSize = poolSize;
        DiContainer = diContainer;

        SpawnGameObjects();
    }
}
