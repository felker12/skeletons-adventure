using RpgLibrary.ItemClasses;

namespace RpgLibrary.GameObjectClasses
{
    public class ChestData
    {
        public List<ItemData> ItemDatas { get; set; } = [];

        public ChestData() { }

        public ChestData(ChestData data)
        {
            ItemDatas = data.ItemDatas;
        }

        public ChestData Clone()
        {
            return new(this);
        }
    }
}
