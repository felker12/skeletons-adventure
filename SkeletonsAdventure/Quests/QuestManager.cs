using RpgLibrary.QuestClasses;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.Quests
{
    internal class QuestManager
    {
        private static Dictionary<string, Quest> Quests = GameManager.QuestsClone;
        //public List<Quest> Quests { get; set; } = [];
        public List<Quest> ActiveQuests { get; set; } = [];
        public List<Quest> CompletedQuests { get; set; } = [];

        internal QuestManager() { }

        public void StartQuest(Quest quest)
        {
            ActiveQuests.Add(quest);
            quest.StartQuest();
        }

        public void CompleteQuest(Quest quest)
        {
            ActiveQuests.Remove(quest);
            CompletedQuests.Add(quest);
            quest.CompleteQuest();
        }

        public QuestManagerData GetQuestManagerData()
        {
            return new QuestManagerData
            {
                QuestsData = [.. Quests.Values.Select(q => q.GetQuestData())],
                ActiveQuestsData = [.. ActiveQuests.Select(q => q.GetQuestData())],
                CompletedQuestsData = CompletedQuests.Select(q => q.GetQuestData()).ToList()
            };
        }

        public void LoadQuestManagerData(QuestManagerData data)
        {
            Quests = data.QuestsData.ToDictionary(q => q.Name, q => new Quest(q));
            ActiveQuests = data.ActiveQuestsData.Select(q => new Quest(q)).ToList();
            CompletedQuests = [.. data.CompletedQuestsData.Select(q => new Quest(q))];
        }

        public override string ToString()
        {
            return
                $"Quests: {string.Join(", ", Quests.Select(q => q.Value.Name))}" +
                $"{Environment.NewLine}" +
                $"Active Quests: {string.Join(", ", ActiveQuests.Select(q => q.Name))}" +
                $"{Environment.NewLine}" +
                $"Completed Quests: {string.Join(", ", CompletedQuests.Select(q => q.Name))}" +
                $"{Environment.NewLine}";
        }
    }
}
