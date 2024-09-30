using System;
using System.Linq;

namespace FantasyBattle
{
    public class Player : Target
    {
        public Inventory Inventory { get; }
        public Stats Stats { get; }

        public Player(Inventory inventory, Stats stats)
        {
            Inventory = inventory;
            Stats = stats;
        }

        public Damage CalculateDamage(Target other)
        {
            int baseDamage = Inventory.GetBaseDamageFromEquipment();
            float damageModifier = CalculateDamageModifier();
            int totalDamage = (int)Math.Round(baseDamage * damageModifier, 0);
            int soak = other.GetSoak(totalDamage);
            return new Damage(Math.Max(0, totalDamage - soak));
        }
        
        private float CalculateDamageModifier()
        {
            float strengthModifier = Stats.Strength * 0.1f;
            float damageModifier = Inventory.GetDamageModifierFromEquipment();
            return strengthModifier + damageModifier;
        }
        
        public override int GetSoak(int? totalDamage)
        {
            // TODO: Not implemented yet
            //  Add friendly fire
            return totalDamage ?? 0;
        }
    }
}