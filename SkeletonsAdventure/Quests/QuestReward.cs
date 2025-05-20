using SkeletonsAdventure.ItemClasses;
using System.Collections.Generic;
using System.Linq;

namespace SkeletonsAdventure.Quests
{
    public class QuestReward
    {
        public int Gold { get; set; } = 0;
        public int Experience { get; set; } = 0;
        public List<GameItem> Items { get; set; } = [];

        public QuestReward()
        {
        }

        public QuestReward(QuestReward questReward)
        {
            Gold = questReward.Gold;
            Experience = questReward.Experience;
            Items = [.. questReward.Items.Select(i => new GameItem(i))]; //TODO test this
        }
    }
}
