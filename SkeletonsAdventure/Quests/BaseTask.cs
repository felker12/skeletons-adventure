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
        public int RequiredAmount { get; set; } = 1;
        public int CompletedAmount { get; set; } = 0;
        public string TaskToComplete { get; set; } = string.Empty;

        public bool IsCompleted => CompletedAmount >= RequiredAmount;
        public string CompletedProgress 
        { 
            get => $"{CompletedAmount} out of {RequiredAmount} completed for task: {TaskToComplete}"; 
        }

        public BaseTask() { }

        public BaseTask(BaseTaskData data)
        {
            RequiredAmount = data.RequiredAmount; 
            CompletedAmount = data.CompletedAmount;
            TaskToComplete = data.TaskToComplete;
        }

        public BaseTask(BaseTask task)
        {
            RequiredAmount = task.RequiredAmount;
            CompletedAmount = task.CompletedAmount;
            TaskToComplete = task.TaskToComplete;
        }

        public void ProgressTask(int amount = 1)
        {
            if (amount < 1)
                throw new ArgumentException("Amount must be at least 1.", nameof(amount));
            CompletedAmount += amount;
            if (CompletedAmount > RequiredAmount)
                CompletedAmount = RequiredAmount; // Ensure we don't exceed the required amount
        }

        public virtual BaseTask Clone()
        { 
            return new BaseTask(this); 
        }

        public BaseTaskData GetBaseTaskData()
        {
            return new()
            {
                RequiredAmount = RequiredAmount,
                CompletedAmount = CompletedAmount,
                TaskToComplete = TaskToComplete
            };
        }

        public override string ToString()
        {
            return CompletedProgress;
        }
    }
}
