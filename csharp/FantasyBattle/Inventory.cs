namespace FantasyBattle
{
    public class Inventory
    {
        public virtual Equipment Equipment { get; }

        public Inventory()
        {

        }


        public Inventory(Equipment equipment)
        {
            Equipment = equipment;
        }
        
        public int GetBaseDamageFromEquipment() // call it CalculateBaseDamage instead
        {
            return Equipment.CalculateBaseDamage();
        }
        
        public float GetDamageModifierFromEquipment()  // call it CalculateDamageModifier instead
        {
            return Equipment.CalculateDamageModifier();
        }
    }
}