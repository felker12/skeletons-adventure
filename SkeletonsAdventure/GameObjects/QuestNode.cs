using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public QuestNode(QuestNodeData obj) : base(obj)
        {

        }

        public override void Update(GameTime gameTime, Rectangle player)
        {
            if (CheckPlayerNear(player))
            {
                HandleInput();
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
        public override void Interact()
        {
            //TODO
            System.Diagnostics.Debug.WriteLine($"Overrided logic");
            base.Interact();
        }
    }
}
