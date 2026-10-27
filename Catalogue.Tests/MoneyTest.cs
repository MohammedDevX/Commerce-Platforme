using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Tests;

public class MoneyTest
{
    [Fact]
    public void Should_Price_BePositive_Test()
    {
        decimal price = 10m;
        Money money = new(price);
        Console.WriteLine("The price format is correcte : " + price);
    }

    [Fact]
    public void Should_Price_Accepte_Zero_Test()
    {
        decimal price = 0;
        Money money = new(price);
        Console.WriteLine("The price accepte zero as value : " + price);
    }

    [Fact]
    public void Should_Throw_Exception_If_Price_Is_Negative_Test()
    {
        decimal price = -1;
        var exception = Assert.Throws<ArgumentException>(() => new Money(price));
        Assert.Equal("Price cannot be lower than 0", exception.Message);
    }

    [Fact]
    public void Should_Two_Prices_With_Same_Value_Be_Equals_Test()
    {
        decimal price = 100m;
        var price1 = new Money(price);
        var price2 = new Money(price);
        Assert.Equal(price1, price2);
        Assert.True(price1 == price2);
    }

    [Fact]
    public void Should_Two_Prices_Not_With_Same_Value_Not_Be_Equals_Test()
    {
        decimal price = 100m;
        decimal diffPrice = 500m;
        var price1 = new Money(price);
        var price2 = new Money(diffPrice);
        Assert.NotEqual(price1, price2);
        Assert.False(price1 == price2);
    }

    [Fact]
    public void Should_Aritmetic_Operations_Valid_Between_Prices_Test()
    {
        decimal price1 = 30;
        decimal price2 = 26;
        Console.WriteLine(new Money(price1).Price + new Money(price2).Price);
        Console.WriteLine(new Money(price1).Price - new Money(price2).Price);
        Console.WriteLine(new Money(price1).Price * new Money(price2).Price);
        Console.WriteLine(new Money(price1).Price / new Money(price2).Price);
    }
}
