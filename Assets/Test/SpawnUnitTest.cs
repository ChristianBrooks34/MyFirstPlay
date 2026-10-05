using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Zenject;

public class SpawnUnitTest
{

    internal class HealthBarStub : HealthBar
    {
        public override void Initialize(IDamageable unit, Health health)
        {

        }
    }

    internal class PlayerStub : Player
    {
        public override void Initialize(PlayerProfile playerProfile)
        {
            UnitProfile = playerProfile;
            UnitData = playerProfile.Data;
            Health = new Health(UnitData);
            HealthBar.Initialize(this, Health);
        }
    }

    internal class EnemyStub : Enemy
    {
        protected override IObjectPool ObjectPool { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        protected override UnitEventManager UnitEventManager { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        protected override GlobalEventManager GlobalEventManager { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        protected override IStateMachine StateMachine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override void Initialize(EnemyProfile enemyProfile)
        {
            UnitProfile = enemyProfile;
            UnitData = enemyProfile.Data;
            Health = new Health(UnitData);
            HealthBar.Initialize(this, Health);
        }
    }

    [Test]
    public void WhenPlayerIsSpawned_AndStartLevel_ThenPlayerShouldNotBeNull()
    {
        // Arrange → Подготовка

        var containerMock = Substitute.For<IInstantiator>();

        SpawnUnitEventManager spawnUnitEventManager = new SpawnUnitEventManager();

        SpawnUnitFactory stawnUnitFactory = new SpawnUnitFactory(containerMock, spawnUnitEventManager);

        GameObject playerGo = new GameObject("PlayerGO");
        playerGo.AddComponent<PlayerStub>();

        GameObject healthBarGO = new GameObject("HealthBarGO");
        healthBarGO.AddComponent<HealthBarStub>();

        containerMock.InstantiatePrefab(Arg.Any<UnityEngine.Object>(), Arg.Any<Vector3>(), Arg.Any<Quaternion>(), Arg.Any<Transform>())
            .Returns(playerGo);

        containerMock.InstantiatePrefab(Arg.Any<UnityEngine.Object>(), Arg.Any<Transform>())
            .Returns(healthBarGO);

        PlayerProfile playerProfile = ScriptableObject.CreateInstance<PlayerProfile>(); ;
        PlayerData playerData = new PlayerData();

        playerProfile.Data = playerData;
        playerProfile.UnitPrefab = playerGo;
        playerProfile.HealthBarPrefab = healthBarGO;

        Vector2 position = new Vector2(0,0);

        Transform parent = new GameObject().transform;

        // Act → Действие
        var player = stawnUnitFactory.PlayerSpawn(playerProfile, position, parent, parent);

        // Assert → Проверка
        player.Should().NotBeNull();
    }


    [Test]
    public void WhenEnemyIsSpawned_AndStartLevel_ThenPlayerShouldNotBeNull()
    {
        // Arrange → Подготовка

        var containerMock = Substitute.For<IInstantiator>();

        SpawnUnitEventManager spawnUnitEventManager = new SpawnUnitEventManager();

        SpawnUnitFactory stawnUnitFactory = new SpawnUnitFactory(containerMock, spawnUnitEventManager);

        GameObject enemyGo = new GameObject("EnemyGO");
        var enemy = enemyGo.AddComponent<EnemyStub>();

        GameObject healthBarGO = new GameObject("HealthBar_PrefabRoot");
        GameObject healthBarChild = new GameObject("HealthBar_ComponentHolder");
        healthBarChild.transform.SetParent(healthBarGO.transform);
        healthBarChild.AddComponent<HealthBarStub>();

        containerMock.InstantiatePrefab(Arg.Any<UnityEngine.Object>(), Arg.Any<Vector3>(), Arg.Any<Quaternion>(), Arg.Any<Transform>())
            .Returns(enemyGo, healthBarGO);

        EnemyProfile enemyProfile = ScriptableObject.CreateInstance<EnemyProfile>(); ;
        EnemyData enemyData = new EnemyData();

        enemyProfile.Data = enemyData;
        enemyProfile.UnitPrefab = enemyGo;
        enemyProfile.HealthBarPrefab = healthBarGO;

        Vector2 position = new Vector2(0, 0);

        Transform parent = new GameObject().transform;

        Player target = new GameObject().AddComponent<PlayerStub>();

        // Act → Действие
        var player = stawnUnitFactory.EnemySpawn(enemyProfile, position, parent, parent, target);

        // Assert → Проверка
        player.Should().NotBeNull();
    }
}
