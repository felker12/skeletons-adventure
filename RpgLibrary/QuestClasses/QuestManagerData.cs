using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.QuestClasses
{
    public class QuestManagerData
    {
        public List<QuestData> QuestsData { get; set; } = new List<QuestData>();
        public List<QuestData> ActiveQuestsData { get; set; } = new List<QuestData>();
        public List<QuestData> CompletedQuestsData { get; set; } = new List<QuestData>();
        public QuestManagerData() { }

        public QuestManagerData(QuestManagerData data)
        {
            QuestsData = data.QuestsData;
            ActiveQuestsData = data.ActiveQuestsData;
            CompletedQuestsData = data.CompletedQuestsData;
        }

        public QuestManagerData Clone()
        {
            return new QuestManagerData(this);
        }

        public override string ToString()
        {
            return 
                $"Quests: {string.Join(", ", QuestsData.Select(q => q.Name))}" +
                $"{Environment.NewLine}" +
                $"Active Quests: {string.Join(", ", ActiveQuestsData.Select(q => q.Name))}" +
                $"{Environment.NewLine}" +
                $"Completed Quests: {string.Join(", ", CompletedQuestsData.Select(q => q.Name))}" +
                $"{Environment.NewLine}";
        }
    }
}
