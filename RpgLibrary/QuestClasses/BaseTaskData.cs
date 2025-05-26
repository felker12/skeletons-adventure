using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.QuestClasses
{
    public class BaseTaskData
    {
        public int RequiredSteps { get; set; } = 1;
        public int CompletedSteps { get; set; } = 0;
        public string TaskToComplete { get; set; } = string.Empty;


        public BaseTaskData() { }

        public override string ToString()
        {
            return $"{CompletedSteps} out of {RequiredSteps} steps completed for task: {TaskToComplete}";
        }
    }
}
