
namespace SkeletonsAdventure.Engines
{
    internal class DropTable
    {
        private List<DropTableItem> DropTableList { get; set; } = [];
        private readonly int _maxDropChance = 100; // Represents the total drop chance, default is 100%
        public string[] ItemNames; // Array to hold item names based on drop chance

        public DropTable() 
        {
            ItemNames = new string[_maxDropChance]; // Initialize the ItemNames array with the size of max drop chance
        }
        public DropTable(List<DropTableItem> dropTableList)
        {
            DropTableList = dropTableList;

            ItemNames = new string[_maxDropChance]; // Initialize the ItemNames array with the size of max drop chance
        }

        public List<DropTableItem> GetDropTableList()
        {
            return DropTableList;
        }

        public int RemainingDropChance()
        {
            return _maxDropChance - TotalDropChance(); // Returns the remaining chance. Used to check if more items can be added.
        }

        public int TotalDropChance()
        {
            int totalChance = 0;
            foreach (var item in DropTableList)
            {
                totalChance += item.DropChance;
            }

            return totalChance;
        }

        public bool AddItem(DropTableItem item)
        {
            if(TotalDropChance() >= 100)
            {
                return false; // Cannot add item if total drop chance is already 100% or more
            }

            DropTableList.Add(item);
            return true; // Item added successfully
        }

        public void RemoveItem(DropTableItem item)
        {
            DropTableList.Remove(item);
        }
    }
}
