using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.Quests;
using System.Collections.Generic;
using System.Linq;

namespace SkeletonsAdventure.GameObjects
{
    internal class QuestNode : InteractableObject
    {
        public List<Quest> Quests { get; set; } = [];

        public Quest LowestRequirementQuest
        {
            get
            {
                if (Quests.Count == 0)
                    return null;
                return Quests.OrderBy(q => q.Requirements.GetTotalRequirements()).FirstOrDefault();
            }
        }

        public Quest DisplayedQuest
        {
            get
            {
                if (Quests.Count == 0)
                    return null;
                return Quests.FirstOrDefault(q => !q.IsCompleted && q.Active);
            }
        }

        public QuestNode(TiledMapObject obj) : base(obj)
        {

        }

        public QuestNode(QuestNode node) : base(node)
        {

        }

        public QuestNode(QuestNodeData data) : base(data)
        {

        }

        public override void Update(GameTime gameTime, Player player)
        {
            if (CheckPlayerNear(player))
            {
                HandleInput(player);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override QuestNode Clone()
        {
            return new(this);
        }

        public override void Interact(Player player)
        {
            foreach (var quest in Quests)
            {
                if (quest.PlayerRequirementsMet(player) is false)
                    continue;

                //check if the quest is already completed or active
                bool completed = false;
                bool active = false;

                foreach (Quest completedQuest in player.CompletedQuests)
                {
                    if (completedQuest.Name == quest.Name)
                    {
                        completed = true;
                        break;
                    }
                }

                if (completed)
                    continue; // Skip to the next quest if this one is already completed

                foreach (Quest activeQuest in player.ActiveQuests)
                {
                    if (activeQuest.Name == quest.Name)
                    {
                        active = true;
                        break;
                    }
                }

                if (active)
                    continue; // Skip to the next quest if this one is already active

                //add the quest to the player's active quests if all the requirements are met
                player.ActiveQuests.Add(quest.Clone());
            }
        }
    }
}
