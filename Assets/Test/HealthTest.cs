using FluentAssertions;
using NUnit.Framework;

public class HealthTest
{
    [Test]
    public void WhenAddOneHealthInHealth_AndHealthIsZero_ThenHealthAddInHealth()
    {
        // Arrange
        var maxHealth = 100;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = 0;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        // Act
        health.Add(1);

        // Assert.
        health.CurrentValue.Should().Be(1);
    }

    [Test]
    public void WhenAddHealth_AndNewValueExceedsMaxHealth_ThenHealthShouldBeMaxHealth()
    {
        // Arrange
        var maxHealth = 100;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = 95;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        // Act
        health.Add(10);

        // Assert.
        health.CurrentValue.Should().Be(maxHealth);
    }


    [Test]
    public void WhenAddHealth_AndHealthIsAlreadyMax_ThenHealthDoesNotChange()
    {
        // Arrange
        var maxHealth = 100;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = maxHealth;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        // Act
        health.Add(10);

        // Assert.
        health.CurrentValue.Should().Be(maxHealth);
    }


    [Test]
    public void WhenSubstractHealth_AndNewValueIsPositive_ThenHealthShouldDecrease()
    {
        // Arrange → Подготовка
        var maxHealth = 100;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = maxHealth;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        // Act → Действие
        health.Substract(10);

        // Assert → Проверка
        health.CurrentValue.Should().Be(90);
    }


    [Test]
    public void WhenSubstractHealth_AndValueExceedsCurrentHealth_ThenHealthShouldBeZero()
    {
        // Arrange → Подготовка
        var maxHealth = 100;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = maxHealth;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        // Act → Действие
        health.Substract(110);

        // Assert → Проверка
        health.CurrentValue.Should().Be(0);
    }

    [Test]
    public void WhenSubstractHealth_AndValueIsNegative_ThenHealthDoesNotChange()
    {
        // Arrange → Подготовка
        var maxHealth = 100;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = maxHealth;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        // Act → Действие
        health.Substract(-10);

        // Assert → Проверка
        health.CurrentValue.Should().Be(maxHealth);
    }

    [Test]
    public void WhenAddHealth_AndValueIsNegative_ThenHealthDoesNotChange()
    {
        // Arrange → Подготовка
        var maxHealth = 100;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = maxHealth;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        // Act → Действие
        health.Add(-10);

        // Assert → Проверка
        health.CurrentValue.Should().Be(maxHealth);
    }

    [Test]
    public void WhenInitializeMethodCalled_AndNewValueIsPassed_ThenHealthChangesAndEventInvokes()
    {
        // Arrange → Подготовка
        var maxHealth = 100;
        var newHealth = 50;
        UnitData unitData = new UnitData();
        unitData.Health.CurrentValue = maxHealth;
        unitData.Health.StartValue = maxHealth;

        var health = new Health(unitData, initializeWithMax: false);

        float eventReceivedValue = -1f;
        bool isEventInvoked = false;

        health.OnChangeHealth += (value) =>
        {
            isEventInvoked = true;
            eventReceivedValue = value;
        };

        // Act → Действие
        health.Initialize(newHealth);

        // Assert → Проверка
        health.CurrentValue.Should().Be(newHealth);
        isEventInvoked.Should().BeTrue();
        eventReceivedValue.Should().Be(newHealth);
    }
}
