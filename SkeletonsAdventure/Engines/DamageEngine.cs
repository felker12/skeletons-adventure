using SkeletonsAdventure.Entities;
using System;

namespace SkeletonsAdventure.Engines
{
    public class DamageEngine() //TODO WIP
    {
        private readonly static Random rnd = new();
        public static int CalculateDamage(Entity attacker, Entity target)
        {
            int dmg, num = 0;
            //add the 1 because the random.Next() will only provide a number less than damage
            dmg = (attacker.attack + attacker.weaponAttack) - (target.defence + target.armourDefence) + 1; 

            if (dmg > 0)
            {
                num = rnd.Next(dmg);
                if (num == 0)
                {
                    num = rnd.Next(dmg);
                }
            }

            return num;
        }
    }
}
