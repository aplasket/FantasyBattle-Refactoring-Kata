using System.Collections.Generic;
using Xunit;

namespace FantasyBattle.Tests;

public class SimpleEnemyTest
{
    [Theory]
    [InlineData(5, 1.0f, 0, 10)]
    [InlineData(-15, 2.5f, 0,-52)] // 52.5 rounds to 52
    [InlineData(100, 5.5, 0, 650)] 
    public void GetTotalSoak_WithArmorAndBuffs_ReturnsCorrectRoundedValue(
        int damageSoak,
        float soakMod,
        float damageMod,
        int expected
    )
    {
        // Arrange
        var simpleArmor = new SimpleArmor(damageSoak);
        var basicBuff = new BasicBuff(soakMod, damageMod);
        var target = new SimpleEnemy(simpleArmor, new List<Buff>(){ basicBuff });

        // Act
        var result = target.GetSoak(null);

        // Assert
        Assert.Equal(expected, result);
    }
    
    // simple enemy cannot have nullable property types for armor and buff
    // consider adding test for null armor and buff, assert throw exception or write method to handle null
}