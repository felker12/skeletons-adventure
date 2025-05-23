using RpgLibrary.QuestClasses;
using SkeletonsAdventure.Engines;
using System.Collections.Generic;
using System.Linq;

namespace SkeletonsAdventure.Quests
{
    internal class Quest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public Requirements Requirements { get; set; } = new();
        public List<Quest> SubQuests { get; set; } = [];

        public Quest() { }

        public Quest(Quest quest)
        {
            Name = quest.Name;
            Description = quest.Description;
            IsCompleted = quest.IsCompleted;
            Requirements = quest.Requirements;
            SubQuests = quest.SubQuests;
        }

        public Quest(QuestData data)
        {
            Name = data.Name;
            Description = data.Description;
            IsCompleted = data.IsCompleted;
            Requirements = new Requirements(data.RequirementData);

            SubQuests = [.. data.SubQuests.Select(q => new Quest(q))]; //TODO test this
        }

        public Quest Clone()
        {
            return new Quest(this);
        }
    }
}
