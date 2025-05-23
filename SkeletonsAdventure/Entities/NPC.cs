﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.Quests;
using System.Collections.Generic;

namespace SkeletonsAdventure.Entities
{
    internal class NPC : AnimatedSprite
    {
        List<Quest> Quests { get; set; } = [];

        public NPC() : base()
        {
        }

        public NPC(NPC npc) : base()
        {
            Quests = npc.Quests;
        }

        public NPC(NPCData data) : base()
        {
            foreach (var questData in data.QuestDatas)
                Quests.Add(new Quest(questData));
        }

        public NPC Clone() 
        { 
            return new NPC(this); 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
