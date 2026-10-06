using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.TestTools;

public class ValueBarTest
{

    [Test]
    public void WhenAddValue_AndCurrentValueIsNull_ThenCurrentValueChanged()
    {
        // Arrange → Подготовка
        var go = new GameObject();
        var valueBar = go.AddComponent<ValueBar>();

        valueBar.MaxValue = 100f;

        float addValue = 1f;

        // Act → Действие
        valueBar.Add(addValue);

        // Assert → Проверка
        valueBar.CurrentValue.Value.Should().Be(addValue);
   }

    [Test]
    public void WhenSubstractValue_AndCurrentValueIsNull_ThenCurrentValueDontChanged()
    {
        // Arrange → Подготовка
        var go = new GameObject();
        var valueBar = go.AddComponent<ValueBar>();

        valueBar.MaxValue = 0;

        valueBar.CurrentValue = new ReactiveProperty<float>();
        valueBar.CurrentValue.Value = 1;

        float substractValue = 1f;

        // Act → Действие
        valueBar.Substract(substractValue);

        // Assert → Проверка
        valueBar.CurrentValue.Value.Should().Be(0);
    }

    [Test]
    public void WhenAddNegativeValue_AndCurrentValueIsNull_ThenCurrentValueDontChanged()
    {
        // Arrange → Подготовка
        var go = new GameObject();
        var valueBar = go.AddComponent<ValueBar>();

        valueBar.CurrentValue = new ReactiveProperty<float>();
        valueBar.CurrentValue.Value = 1;

        float addValue = -1f;

        LogAssert.Expect(LogType.Error, "Параметр value не может быть меньше нуля");

        // Act → Действие
        valueBar.Add(addValue);

        // Assert → Проверка
        valueBar.CurrentValue.Value.Should().Be(1);
    }

    [Test]
    public void WhenSubstractNegativeValue_AndCurrentValueIsNull_ThenCurrentValueDontChanged()
    {
        // Arrange → Подготовка
        var go = new GameObject();
        var valueBar = go.AddComponent<ValueBar>();

        float substractValue = -1f;

        LogAssert.Expect(LogType.Error, "Параметр value не может быть меньше нуля");

        // Act → Действие
        valueBar.Substract(substractValue);

        // Assert → Проверка
        valueBar.CurrentValue.Value.Should().Be(0);
    }

    [Test]
    public void WhenAddValue_AndNewValueExceedsMaxValue_ThenCurrentValueClampedToMax()
    {
        // Arrange → Подготовка
        var go = new GameObject();
        var valueBar = go.AddComponent<ValueBar>();

        valueBar.MaxValue = 100f;

        float addValue = 200f;

        // Act → Действие
        valueBar.Add(addValue);

        // Assert → Проверка
        valueBar.CurrentValue.Value.Should().Be(valueBar.MaxValue);
    }

    [Test]
    public void WhenSubstractValue_AndNewValueDropsBelowMinValue_ThenCurrentValueClampedToMin()
    {
        // Arrange → Подготовка
        var go = new GameObject();
        var valueBar = go.AddComponent<ValueBar>();

        valueBar.MinValue = 0;

        valueBar.CurrentValue = new ReactiveProperty<float>();
        valueBar.CurrentValue.Value = 100;

        float substractValue = 200f;

        // Act → Действие
        valueBar.Substract(substractValue);

        // Assert → Проверка
        valueBar.CurrentValue.Value.Should().Be(valueBar.MinValue);
    }
}

