﻿using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemClasses
{
    internal class EquippedItems()
    {
        public GameItem Mainhand, Offhand, BodySlot, HeadSlot, HandsSlot, FeetSlot;

        public int EquippedItemsAttackBonus()
        {
            int att = 0;

            if (Mainhand != null && Mainhand is Weapon weapon)
                att = weapon.AttackValue;

            return att;
        }

        public int EquippedItemsDefenceBonus()
        {
            int def = 0;

            if (HeadSlot != null && HeadSlot is Armor head)
                def += head.DefenseValue;
            if (BodySlot != null && BodySlot is Armor body)
                def += body.DefenseValue;
            if(HandsSlot != null && HandsSlot is Armor hands)
                def += hands.DefenseValue;
            if (FeetSlot != null && FeetSlot is Armor feet)
                def += feet.DefenseValue;

            return def;
        }

        public int EquippedItemsManaBonus()
        {
            //int mana = 0;
            //if (HeadSlot != null && HeadSlot.BaseItem is Armor head)
            //    mana += head.ManaValue;
            //if (BodySlot != null && BodySlot.BaseItem is Armor body)
            //    mana += body.ManaValue;
            //if (HandsSlot != null && HandsSlot.BaseItem is Armor hands)
            //    mana += hands.ManaValue;
            //if (FeetSlot != null && FeetSlot.BaseItem is Armor feet)
            //    mana += feet.ManaValue;
            //return mana;

            //TODO: Implement mana bonus for equipped items
            return 0; // Placeholder for future mana bonus implementation
        }

        public int EquippedItemsHealthBonus()
        {
            //int health = 0;
            //if (HeadSlot != null && HeadSlot.BaseItem is Armor head)
            //    health += head.HealthValue;
            //if (BodySlot != null && BodySlot.BaseItem is Armor body)
            //    health += body.HealthValue;
            //if (HandsSlot != null && HandsSlot.BaseItem is Armor hands)
            //    health += hands.HealthValue;
            //if (FeetSlot != null && FeetSlot.BaseItem is Armor feet)
            //    health += feet.HealthValue;
            //return health;

            //TODO: Implement health bonus for equipped items
            return 0; // Placeholder for future health bonus implementation
        }

        public void TryEquipItem(GameItem gameItem)
        {
            if(gameItem is Weapon weapon)
            {
                if (weapon.NumberHands == Hands.Both)
                {
                    if (Mainhand != null)
                        TryUnequipItem(Mainhand);
                    if (Offhand != null)
                        TryUnequipItem(Offhand);

                    Mainhand = gameItem;
                }
                else if (weapon.NumberHands == Hands.Main)
                {
                    if (Mainhand != null)
                        TryUnequipItem(Mainhand);

                    Mainhand = gameItem;
                }
                else if (weapon.NumberHands == Hands.Off)
                {
                    if (Offhand != null)
                        TryUnequipItem(Offhand);
                    //If a mainhand weapon is equipped and it is a 2 handed weapon unequip it
                    if (Mainhand != null && Mainhand is Weapon weap && weap.NumberHands == Hands.Both) 
                        TryUnequipItem(Mainhand);

                    Offhand = gameItem;
                }

                weapon.SetEquipped(true);
            }
            else if (gameItem is Armor armor)
            {
                switch (armor.ArmorLocation)
                {
                    case ArmorLocation.Head:
                        if (HeadSlot != null)
                            TryUnequipItem(HeadSlot);
                        HeadSlot = gameItem;
                        break;
                    case ArmorLocation.Body:
                        if (BodySlot != null)
                            TryUnequipItem(BodySlot);
                        BodySlot = gameItem;
                        break;
                    case ArmorLocation.Hands:
                        if (HandsSlot != null)
                            TryUnequipItem(HandsSlot);
                        HandsSlot = gameItem;
                        break;
                    case ArmorLocation.Feet:
                        if (FeetSlot != null)
                            TryUnequipItem(FeetSlot);
                        FeetSlot = gameItem;
                        break;
                }

                armor.SetEquipped(true);
            }
        }

        public void TryUnequipItem(GameItem gameItem)
        {
            if (gameItem == Mainhand)
            {
                Mainhand = null;
            }
            else if (gameItem == Offhand)
            {
                Offhand = null;
            }
            else if (gameItem == HeadSlot)
            {
                HeadSlot = null;
            }
            else if (gameItem == BodySlot)
            {
                BodySlot = null;
            }
            else if (gameItem == HandsSlot)
            {
                HandsSlot = null;
            }
            else if (gameItem == FeetSlot)
            {
                FeetSlot = null;
            }
            else return;

            if(gameItem is Armor armor)
                armor.SetEquipped(false);
            if(gameItem is Weapon weapon)
                weapon.SetEquipped(false);
        }
    }
}
