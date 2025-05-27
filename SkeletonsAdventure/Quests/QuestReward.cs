using RpgLibrary.ItemClasses;
using RpgLibrary.QuestClasses;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.ItemClasses;
using System.Collections.Generic;
using System.Linq;

namespace SkeletonsAdventure.Quests
{
    internal class QuestReward
    {
        public int XP { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public List<GameItem> Items { get; set; } = [];

        public QuestReward()
        {
        }

        public QuestReward(QuestReward questReward)
        {
            XP = questReward.XP;
            Gold = questReward.Gold;
            Items = [.. questReward.Items.Select(i => new GameItem(i))];
        }

        public QuestReward(QuestRewardData data)
        {
            XP = data.XP;
            Gold = data.Gold;

            foreach (ItemData itemData in data.Items)
            {


                //BaseItem baseItem = new(itemData); //TODO remove baseItem and use itemData directly if possible

                GameItem item = GameManager.LoadGameItemFromItemData(itemData);


                Items.Add(item); 
            }
        }

        public QuestReward Clone()
        {
            return new QuestReward(this);
        }

        public QuestRewardData GetQuestRewardData()
        {
            return new QuestRewardData
            {
                XP = XP,
                Gold = Gold,
                Items = [.. Items.Select(i => i.GetItemData())]
            };
        }

        public override string ToString()
        {
            return 
                $"XP: {XP}, " +
                $"Gold: {Gold}, " +
                $"Items: {string.Join(", ", Items.Select(i => i.Name))}"; // Assuming GameItem has a Name property
        }
    }
}
