using Moq;
using NUnit.Framework;
using System;
using UnityEngine;
using Zenject;

public class SpawnUnitTest
{
    [Test]
    public void WhenPlayerIsSpawned_AndStartLevel_ThenPlayerShouldNotBeNull()
    {
        Debug.Log("Run test");

        // »спользуем Loose дл€ отладки
        var mockContainer = new Mock<IDiContainer>(MockBehavior.Loose);
        var mockEventManager = new Mock<SpawnUnitEventManager>();

        var playerGO = new GameObject();
        var healthBarGO = new GameObject();

        Debug.Log(1);

        try
        {

            mockContainer
                .Setup(c => c.InstantiatePrefab(
                    It.IsAny<GameObject>(),
                    It.IsAny<Vector3>(),
                    It.IsAny<Quaternion>(),
                    It.IsAny<Transform>()))
                .Returns(playerGO);
            Debug.Log("Setup 1 completed");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Setup 1 failed: {ex.Message}");
            throw;
        }
        Debug.Log(2);

        mockContainer
            .Setup(c => c.InstantiatePrefab(
                It.IsAny<GameObject>(),
                It.IsAny<Transform>()))
            .Returns(healthBarGO);

        //var spawnUnitFactory = new SpawnUnitFactory(mockContainer.Object, mockEventManager.Object);
        //Debug.Log(23);

        //var playerProfile = new PlayerProfile
        //{
        //    //Prefab = new GameObject(),
        //    //HealthBarPrefab = new GameObject()
        //};

        //var parent = new GameObject().transform;

        //// Act Ч вызываем тестируемый метод
        //var player = spawnUnitFactory.PlayerSpawn(playerProfile, Vector2.zero, parent, parent);

        // Assert Ч провер€ем результат
        //Assert.IsNotNull(player, "PlayerSpawn должен вернуть не-null объект Player");
        //Assert.AreEqual(playerGO, player.PlayerContext, "PlayerContext должен ссылатьс€ на созданный GameObject");
        //Assert.IsNotNull(player.HealthBar, "HealthBar должен быть инициализирован");
    }

    public class DiContainerAdapter : IDiContainer
    {
        private readonly DiContainer _container;

        public DiContainerAdapter(DiContainer container) => _container = container;

        public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) =>
            _container.InstantiatePrefab(prefab, position, rotation, parent);

        public GameObject InstantiatePrefab(GameObject prefab, Transform parent) =>
            _container.InstantiatePrefab(prefab, parent);
    }
}
