using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WalletTest
{

    [Test]
    public void WhenAddOneMoneyInWallet_AndMoneyInWalletIsZero_ThenMoneyAddInWallet()
    {
        // Arrange
        var initialBalance = 0;
        var amountToAdd = 1;
        var wallet = new Wallet(initialBalance);

        // Act
        wallet.Add(amountToAdd);

        // Assert.
        wallet.Money.Should().Be(1);
    }

    [Test]
    public void WhenAddZeroMoneyToWallet_AndInitialBalanceIsZero_ThenBalanceDoesNotChange()
    {
        // Arrange
        var initialBalance = 0;
        var amountToAdd = 0;
        var wallet = new Wallet(initialBalance);

        // Act
        wallet.Add(amountToAdd);

        // Assert.
        wallet.Money.Should().Be(0);
    }

    [Test]
    public void WhenSubstractOneMoneyToWallet_AndInitialBalanceIsOne_ThenBalanceIsZero()
    {
        // Arrange
        var initialBalance = 1;
        var amountToSubstract = 1;
        var wallet = new Wallet(initialBalance);

        // Act
        wallet.Substract(amountToSubstract);

        // Assert.
        wallet.Money.Should().Be(0);
    }


    [Test]
    public void WhenSubstractMoneyToWallet_AndValueExceedsCurrentBalance_ThenBalanceShouldBeZero()
    {
        // Arrange
        var initialBalance = 100;
        var amountToSubstract = 120;
        var wallet = new Wallet(initialBalance);

        // Act
        wallet.Substract(amountToSubstract);

        // Assert.
        wallet.Money.Should().Be(initialBalance);
    }

    [Test]
    public void WhenSubstractMoneyToWallet_AndValueIsNegative_ThenBalanceDoesNotChange()
    {
        // Arrange
        var initialBalance = 1;
        var amountToSubstract = -1;
        var wallet = new Wallet(initialBalance);

        LogAssert.Expect(LogType.Error, "Параметр value не может быть меньше нуля");

        // Act
        wallet.Substract(amountToSubstract);

        // Assert.
        wallet.Money.Should().Be(initialBalance);
    }

    [Test]
    public void WhenAddMoneyToWallet_AndValueIsNegative_ThenBalanceDoesNotChange()
    {
        // Arrange
        var initialBalance = 1;
        var amountToSubstract = -1;
        var wallet = new Wallet(initialBalance);

        LogAssert.Expect(LogType.Error, "Параметр value не может быть меньше нуля");

        // Act
        wallet.Add(amountToSubstract);

        // Assert.
        wallet.Money.Should().Be(initialBalance);
    }
}

