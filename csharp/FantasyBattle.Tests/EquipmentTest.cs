using System;
using Xunit;

namespace FantasyBattle.Tests;

public class EquipmentTest
{
    [Theory]
    [InlineData(-20, -10, -2, -5, -1)]
    [InlineData(0, 16, 25, 18, 31)]
    [InlineData(100, 300, 234, 123, 780)]
    [InlineData(0, 0, 0, 0, 0)]
    public void CalculateBaseDamage_WithFullInventory_ShouldReturnSumOfEquipmentDamage(
        int leftBaseDamage, 
        int rightBaseDamage, 
        int headBaseDamage, 
        int feetBaseDamage, 
        int chestBaseDamage
        )
    {
        // arrange
        var leftHand = new BasicItem("leftHand", leftBaseDamage, 0 );
        var rightHand = new BasicItem("rightHand", rightBaseDamage, 0 );
        var head = new BasicItem("head", headBaseDamage, 0 );
        var feet = new BasicItem("feet", feetBaseDamage, 0 );
        var chest = new BasicItem("chest", chestBaseDamage, 0 );
        var equipment = new Equipment(leftHand, rightHand, head, feet, chest);

        // act
        var result = equipment.CalculateBaseDamage();
        
        // assert
        var expected = leftBaseDamage + rightBaseDamage + headBaseDamage + feetBaseDamage + chestBaseDamage;
        Assert.Equal(expected, result);
    }
    
    [Fact] // should not happen, item and item properties are not set to a nullable property types
    public void CalculateBaseDamage_WhenNullItems_ShouldReturnZero()
    {
        // arrange
        var equipment = new Equipment(null, null, null, null, null);

        // act
        var result = equipment.CalculateBaseDamage();
        
        // assert
        Assert.Equal(0, result);
    }
    
    [Theory]
    [InlineData(-0.9, 0.4, 0.2, -1, -1)] 
    [InlineData(0, 0,0,0,0)]
    [InlineData(1, 8, 2, 2, 2)]
    public void CalculateDamageModifier_InventoryHasDamageModifiers_ReturnsSumOfEquipmentModifiers(
        float leftDamageMod, 
        float rightDamageMod, 
        float headDamageMod, 
        float feetDamageMod, 
        float chestDamageMod
    )
    {
        // arrange
        var leftHand = new BasicItem("leftHand", 0, leftDamageMod );
        var rightHand = new BasicItem("rightHand", 16, rightDamageMod );
        var head = new BasicItem("head", 25, headDamageMod );
        var feet = new BasicItem("feet", 18, feetDamageMod );
        var chest = new BasicItem("chest", 30, chestDamageMod );
        var equipment = new Equipment(leftHand, rightHand, head, feet, chest);
        
        // act
        var result = equipment.CalculateDamageModifier();
        
        // assert
        var expected = leftDamageMod + rightDamageMod + headDamageMod + feetDamageMod + chestDamageMod;
        Assert.Equal(expected, result);
    }
    
    [Fact] // should not happen, item and item properties are not set to a nullable property types
    public void CalculateDamageModifier_WhenNullItems_ShouldReturnZero()
    {
        // arrange
        var equipment = new Equipment(null, null, null, null, null);

        // act
        var result = equipment.CalculateDamageModifier();
        
        // assert
        Assert.Equal(0, result);
        // Assert.Throws<NullReferenceException>(() => result);
        // throwing a null reference exception would be better since refactoring
        // if this was a client's work, we'd want to check with them before changing the 
        // code structure to return 0 and not a null reference exception.
    }

}