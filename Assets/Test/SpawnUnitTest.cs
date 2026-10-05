using FluentAssertions;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.Profiling;
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
}
