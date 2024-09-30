using System;
using System.Collections.Generic;
using System.Linq;

namespace FantasyBattle
{
    public class SimpleEnemy : Target
    {
        public virtual Armor Armor { get; }
        public virtual List<Buff> Buffs { get; }

        public SimpleEnemy()
        {

        }

        public SimpleEnemy(Armor armor, List<Buff> buffs)
        {
            Armor = armor;
            Buffs = buffs;
        }

        public override int GetSoak( int? totalDamage)
        {
            return (int)Math.Round(
                Armor.DamageSoak *
                (
                    Buffs.Select(x => x.SoakModifier).Sum() + 1
                ), 0
            );
        }
    }

    public interface Buff
    {
        float SoakModifier { get; }
        float DamageModifier { get; }
    }

    public interface Armor
    {
        int DamageSoak { get; }
    }
}