using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.Quests
{
    internal class SlayTask : BaseTask
    {
        public string MonsterToSlay { get; set; } = string.Empty;

        public SlayTask() { }

        public SlayTask(SlayTask task) : base(task)
        {
            MonsterToSlay = task.MonsterToSlay;
        }

        public override SlayTask Clone()
        {
            return new SlayTask(this);
        }

    }
}
