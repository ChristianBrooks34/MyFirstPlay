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
        
        var health = new Health(unitData, initializeWithMax : false);

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
}
