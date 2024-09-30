using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;

namespace FantasyBattle.Tests
{
    public class PlayerTest
    {

        // choose this one if you are familiar with mocks
        [Fact(Skip = "Test is not finished yet")]
        public void DamageCalculationsWithMocks() {
            var inventory = new Mock<Inventory>();
            var stats = new Mock<Stats>();
            var target = new Mock<SimpleEnemy>();

            var damage = new Player(inventory.Object, stats.Object).CalculateDamage(target.Object);
            Assert.Equal(10, damage.Amount);
        }

        [Fact]
        public void CalculateDamage_HasAllZeroInventoryStats_ShouldReturnZeroDamage() {
            // arrange
            var item = new BasicItem("item", 0, 0 );
            var equipment = new Equipment(item, item, item, item, item);
            Inventory inventory = new Inventory(equipment);
            Stats stats = new Stats(0);
            SimpleEnemy target = new SimpleEnemy(new SimpleArmor(0), new List<Buff>());
            
            var player = new Player(inventory, stats);
            
            // act
            Damage damage = player.CalculateDamage(target);
            
            // assert
            Assert.Equal(0, damage.Amount);
        }
        
        [Theory]
        [InlineData(-20, -10, -2, -5, -1, 0)]
        [InlineData(0, 16, 25, 18, 31, 90)]
        [InlineData(100, 300, 234, 123, 780, 1537)]
        public void CalculateDamage_IsFullyEquipped_ShouldReturnSumOfEquipmentDamage(
            int leftBaseDamage, 
            int rightBaseDamage, 
            int headBaseDamage, 
            int feetBaseDamage, 
            int chestBaseDamage,
            int expectedTotalDamage
            )
        {
            // arrange
            var leftHand = new BasicItem("leftHand", leftBaseDamage, 0 );
            var rightHand = new BasicItem("rightHand", rightBaseDamage, 0 );
            var head = new BasicItem("head", headBaseDamage, 0 );
            var feet = new BasicItem("feet", feetBaseDamage, 0 );
            var chest = new BasicItem("chest", chestBaseDamage, 0 );
            var equipment = new Equipment(leftHand, rightHand, head, feet, chest);
            Inventory inventory = new Inventory(equipment);
            Stats stats = new Stats(10);
            SimpleEnemy target = new SimpleEnemy(new SimpleArmor(0), new List<Buff>());
            
            var player = new Player(inventory, stats);
            
            // act
            Damage damage = player.CalculateDamage(target);
            
            // assert

            Assert.Equal(expectedTotalDamage, damage.Amount);
        }
        
        [Theory]
        [InlineData(-41, 0)] // total damage cannot be negative
        [InlineData(23, 205)] // 204.7 rounds to 205
        [InlineData(0, 0)]
        [InlineData(150, 1335)]
        public void CalculateDamage_PlayerHasStrength_EquipmentDamageEffectedByStrength(int strength, int expected)
        {
            // arrange
            var leftHand = new BasicItem("leftHand", 0, 0 );
            var rightHand = new BasicItem("rightHand", 16, 0 );
            var head = new BasicItem("head", 25, 0 );
            var feet = new BasicItem("feet", 18, 0 );
            var chest = new BasicItem("chest", 30, 0 );
            var equipment = new Equipment(leftHand, rightHand, head, feet, chest);
            Inventory inventory = new Inventory(equipment);
            Stats stats = new Stats(strength);
            SimpleEnemy target = new SimpleEnemy(new SimpleArmor(0), new List<Buff>());

            
            var player = new Player(inventory, stats);
            
            // act
            Damage damage = player.CalculateDamage(target);
            
            // assert
            Assert.Equal(expected, damage.Amount);
        }
        
        [Theory]
        [InlineData(-0.9, 0.4, 0.2, -1, -1, 0)] // total damage cannot be negative
        [InlineData(0, 0,0,0,0, 0)]
        [InlineData(1, 8, 2, 2, 2, 1335)]
        public void CalculateDamage_InventoryHasDamageModifiers_EquipmentDamageEffectedByModifiers(
            float leftDamageMod, 
            float rightDamageMod, 
            float headDamageMod, 
            float feetDamageMod, 
            float chestDamageMod, 
            int expected
            )
        {
            // arrange
            var leftHand = new BasicItem("leftHand", 0, leftDamageMod );
            var rightHand = new BasicItem("rightHand", 16, rightDamageMod );
            var head = new BasicItem("head", 25, headDamageMod );
            var feet = new BasicItem("feet", 18, feetDamageMod );
            var chest = new BasicItem("chest", 30, chestDamageMod );
            var equipment = new Equipment(leftHand, rightHand, head, feet, chest);
            Inventory inventory = new Inventory(equipment);
            Stats stats = new Stats(0);
            SimpleEnemy target = new SimpleEnemy(new SimpleArmor(0), new List<Buff>());
            
            var player = new Player(inventory, stats);
            
            // act
            Damage damage = player.CalculateDamage(target);
            
            // assert
            Assert.Equal(expected, damage.Amount);
        }
        
        [Fact]
        public void CalculateDamage_TargetIsAnotherPlayer_TotalDamageIsZero()
        {
            // arrange
            var leftHand = new BasicItem("leftHand", 0, 0 );
            var rightHand = new BasicItem("rightHand", 16, 0 );
            var head = new BasicItem("head", 25, 0 );
            var feet = new BasicItem("feet", 18, 0 );
            var chest = new BasicItem("chest", 30, 0 );
            var equipment = new Equipment(leftHand, rightHand, head, feet, chest);
            var inventory = new Inventory(equipment);
            var stats = new Stats(10);
            var target = new Player(null, null);
            
            var player = new Player(inventory, stats);
            
            // act
            Damage damage = player.CalculateDamage(target);
            
            // assert
            Assert.Equal(0, damage.Amount);
        }
        
        [Theory]
        [InlineData(5, 1.0f, 0, 79)]
        [InlineData(-15, 2.5f, 0, 141)] // 141.5 rounds to 141, damage minus negative number equals higher damage
        [InlineData(100, 5.5, 0, 0)] // soak is greater than base damage, 0 is the lowest possible damage, damage cannot be negative
        public void CalculateDamage_TargetSimpleEnemyHasArmorAndBuffs_TotalDamageEffectedByEnemySoakModifiers(int damageSoak, float soakMod, float damageMod, int expected)
        {
            // arrange
            var leftHand = new BasicItem("leftHand", 0, 0 );
            var rightHand = new BasicItem("rightHand", 16, 0 );
            var head = new BasicItem("head", 25, 0 );
            var feet = new BasicItem("feet", 18, 0 );
            var chest = new BasicItem("chest", 30, 0 );
            var equipment = new Equipment(leftHand, rightHand, head, feet, chest);
            var inventory = new Inventory(equipment);
            var stats = new Stats(10);
            var simpleArmor = new SimpleArmor(damageSoak);
            var basicBuff = new BasicBuff(soakMod, damageMod);
            var target = new SimpleEnemy(simpleArmor, new List<Buff>(){ basicBuff });
            
            var player = new Player(inventory, stats);
            
            // act
            Damage damage = player.CalculateDamage(target);
            
            // assert
            Assert.Equal(expected, damage.Amount);
        }
        
        [Fact]
        public void CalculateDamage_CannotBeNegative_ShouldReturnZero()
        {
            // arrange
            var leftHand = new BasicItem("leftHand", 0, 0 );
            var rightHand = new BasicItem("rightHand", 16, 0 );
            var head = new BasicItem("head", 25, 0 );
            var feet = new BasicItem("feet", 18, 0 );
            var chest = new BasicItem("chest", 30, 0 );
            var equipment = new Equipment(leftHand, rightHand, head, feet, chest);
            var inventory = new Inventory(equipment);
            var stats = new Stats(10);
            var simpleArmor = new SimpleArmor(100);
            var basicBuff = new BasicBuff(5.5f, 0);
            var target = new SimpleEnemy(simpleArmor, new List<Buff>(){ basicBuff });
            
            var player = new Player(inventory, stats);
            
            // act
            Damage damage = player.CalculateDamage(target);
            
            // assert
            Assert.Equal(0, damage.Amount);
        }
    }
}
