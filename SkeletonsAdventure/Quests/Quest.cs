using RpgLibrary.QuestClasses;
using System.Collections.Generic;
using System.Linq;

namespace SkeletonsAdventure.Quests
{
    internal class Quest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public bool Active { get; set; } = false;
        public Requirements Requirements { get; set; } = new();
        public List<string> RequiredQuestNames { get; set; } = [];
        public List<BaseTask> Tasks { get; set; } = [];

        public Quest() { }

        public Quest(Quest quest)
        {
            Name = quest.Name;
            Description = quest.Description;
            IsCompleted = quest.IsCompleted;
            Active = quest.Active;
            Requirements = quest.Requirements;
            RequiredQuestNames = quest.RequiredQuestNames;
            Tasks = quest.Tasks;
        }

        public Quest(QuestData data)
        {
            Name = data.Name;
            Description = data.Description;
            IsCompleted = data.IsCompleted;
            Active = data.Active;
            Requirements = new Requirements(data.RequirementData);

            RequiredQuestNames = [.. data.RequiredQuestNameData.Select(q => q)]; //TODO test this
            Tasks = [.. data.BaseTasksData.Select(t => new BaseTask(t))];
        }

        public Quest Clone()
        {
            return new Quest(this);
        }

        public void StartQuest()
        {
            Active = true;
        }

        public void CompleteQuest()
        {
            IsCompleted = true;
            Active = false;
        }

        public QuestData GetQuestData()
        {
            return new QuestData
            {
                Name = Name,
                Description = Description,
                IsCompleted = IsCompleted,
                Active = Active,
                RequirementData = Requirements.GetRequirementData(),
                RequiredQuestNameData = RequiredQuestNames,
                BaseTasksData = GetBaseTaskDatas(),
            };
        }

        public override string ToString()
        {
            return $"Name: {Name}, " +
                   $"Description: {Description}, " +
                   $"IsCompleted: {IsCompleted}, " +
                   $"Active: {Active}, " +
                   $"Requirements: {Requirements}, " +
                   $"Required Quests: {RequiredQuestsToString()}" +
                   $"Tasks: {TasksToString()}";
        }

        public List<BaseTaskData> GetBaseTaskDatas()
        {
            return [.. Tasks.Select(t => t.GetBaseTaskData())];
        }

        public string RequiredQuestsToString()
        {
            string requiredQuests = string.Empty;

            if (RequiredQuestNames == null || RequiredQuestNames.Count == 0)
                return "No required quests.";

            return string.Join(", ", RequiredQuestNames.Select(q => q));
        }

        public string TasksToString()
        {
            if (Tasks == null || Tasks.Count == 0)
                return "No tasks.";

            return string.Join(", ", Tasks.Select(t => t.ToString()));
        }
    }
}
