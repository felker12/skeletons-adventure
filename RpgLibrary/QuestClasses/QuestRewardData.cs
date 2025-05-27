using RpgLibrary.ItemClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.QuestClasses
{
    public class QuestRewardData
    {
        public int XP { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public List<ItemData> Items { get; set; } = new();

        public QuestRewardData() { }

        public QuestRewardData(QuestRewardData data)
        {
            XP = data.XP;
            Gold = data.Gold;
            Items = data.Items;
        }

        public QuestRewardData Clone()
        {
            return new QuestRewardData(this);
        }

        public override string ToString()
        {
            return
                $"XP: {XP}, " +
                $"Gold: {Gold}, " +
                $"Items: {string.Join(", ", Items)}";
        }
    }
}
