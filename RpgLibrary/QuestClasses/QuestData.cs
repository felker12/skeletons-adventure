using RpgLibrary.WorldClasses;

namespace RpgLibrary.QuestClasses
{
    public class QuestData
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public RequirementData RequirementData { get; set; } = new();
        public List<QuestData> SubQuests { get; set; } = new();

        public QuestData() { }

        public QuestData(QuestData data)
        {
            Name = data.Name;
            Description = data.Description;
            IsCompleted = data.IsCompleted;
            RequirementData = data.RequirementData;
            SubQuests = data.SubQuests;
        }

        public QuestData Clone()
        {
            return new QuestData(this);
        }

        public override string ToString()
        {
            return $"Name: {Name}, " +
                $"Description: {Description}, " +
                $"IsCompleted: {IsCompleted}, " +
                $"Requirements: {RequirementData.ToString()}"; //TODO test this 
        }
    }
}
