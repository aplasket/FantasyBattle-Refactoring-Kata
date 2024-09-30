namespace FantasyBattle
{
    public class Equipment
    {
        // TODO add a ring item that may be equipped
        //  that may also add damage modifier
        public Item LeftHand { get; }
        public Item RightHand { get; }
        public Item Head { get; }
        public Item Feet { get; }
        public Item Chest { get; }

        public Equipment(Item leftHand, Item rightHand, Item head, Item feet, Item chest)
        {
            LeftHand = leftHand;
            RightHand = rightHand;
            Head = head;
            Feet = feet;
            Chest = chest;
        }

        public int CalculateBaseDamage()
        {
            return GetBaseDamage(LeftHand) +
                   GetBaseDamage(RightHand) +
                   GetBaseDamage(Head) +
                   GetBaseDamage(Feet) +
                   GetBaseDamage(Chest);
        }

        private int GetBaseDamage(Item item)
        {
            return item?.BaseDamage ?? 0;
        }
        
        public float CalculateDamageModifier()
        {
            return GetDamageModifier(LeftHand) +
                   GetDamageModifier(RightHand) +
                   GetDamageModifier(Head) +
                   GetDamageModifier(Feet) +
                   GetDamageModifier(Chest);
        }

        private float GetDamageModifier(Item item)
        {
            return item?.DamageModifier ?? 0;
        }
    }
}