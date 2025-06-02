using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemClasses
{
    internal class EquippedItems()
    {
        public GameItem Mainhand, Offhand, BodySlot, HeadSlot, HandsSlot, FeetSlot;

        public int EquippedItemsAttackBonus()
        {
            int att = 0;

            if (Mainhand != null && Mainhand.BaseItem is Weapon weapon)
                att = weapon.AttackValue;

            return att;
        }

        public int EquippedItemsDefenceBonus()
        {
            int def = 0;

            if (HeadSlot != null && HeadSlot.BaseItem is Armor head)
                def += head.DefenseValue;
            if (BodySlot != null && BodySlot.BaseItem is Armor body)
                def += body.DefenseValue;
            if(HandsSlot != null && HandsSlot.BaseItem is Armor hands)
                def += hands.DefenseValue;
            if (FeetSlot != null && FeetSlot.BaseItem is Armor feet)
                def += feet.DefenseValue;

            return def;
        }

        public void TryEquipItem(GameItem gameItem)
        {
            BaseItem item = gameItem.BaseItem;
            if(item is Weapon weapon)
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
                    if (Mainhand != null && Mainhand.BaseItem is Weapon weap && weap.NumberHands == Hands.Both) 
                        TryUnequipItem(Mainhand);

                    Offhand = gameItem;
                }

                gameItem.SetEquipped(true);
            }
            else if (item is Armor armor)
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
                gameItem.SetEquipped(true);
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

            gameItem.SetEquipped(false);
        }
    }
}
