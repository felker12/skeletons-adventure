using RpgLibrary.WorldClasses;

namespace SkeletonsAdventure.Engines
{
    internal class Requirements
    {
        public int Level { get; set; } = 0;
        public int Defence { get; set; } = 0;
        public int Attack { get; set; } = 0;

        public Requirements()
        {
        }

        public Requirements(Requirements requirements)
        {
            Level = requirements.Level;
            Defence = requirements.Defence;
            Attack = requirements.Attack;
        }

        public Requirements(RequirementData data)
        {
            Level = data.Level;
            Defence = data.Defence;
            Attack = data.Attack;
        }

        public Requirements Clone()
        {
            return new Requirements(this);
        }
    }
}
