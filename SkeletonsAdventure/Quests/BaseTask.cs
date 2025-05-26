using RpgLibrary.QuestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.Quests
{
    internal class BaseTask
    {
        public int RequiredSteps { get; set; } = 1;
        public int CompletedSteps { get; set; } = 0;
        public string TaskToComplete { get; set; } = string.Empty;

        public bool IsCompleted => CompletedSteps >= RequiredSteps;
        public string CompletedProgress 
        { 
            get => $"{CompletedSteps} out of {RequiredSteps} steps completed for task: {TaskToComplete}"; 
        }

        public BaseTask() { }

        public BaseTask(BaseTaskData data)
        {
            RequiredSteps = data.RequiredSteps; 
            CompletedSteps = data.CompletedSteps;
            TaskToComplete = data.TaskToComplete;
        }

        public BaseTask(BaseTask task)
        {
            RequiredSteps = task.RequiredSteps;
            CompletedSteps = task.CompletedSteps;
            TaskToComplete = task.TaskToComplete;
        }

        public BaseTask Clone()
        { 
            return new BaseTask(this); 
        }

        public BaseTaskData GetBaseTaskData()
        {
            return new()
            {
                RequiredSteps = RequiredSteps,
                CompletedSteps = CompletedSteps,
                TaskToComplete = TaskToComplete
            };
        }

        public override string ToString()
        {
            return CompletedProgress;
        }
    }
}
