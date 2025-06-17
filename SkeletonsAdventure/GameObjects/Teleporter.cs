using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkeletonsAdventure.GameObjects
{
    public class Teleporter
    {
        public string Name { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public Vector2 Destination { get; set; } = new();
        public bool IsActive { get; set; } = true;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public int Width { get; set; } = 32;
        public int Height { get; set; } = 32;
        public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, Width, Height);
        public List<string> RequiredQuestNames { get; set; } = [];
        public Requirements Requirements { get; set; } = new();
        public Label Info { get; set; } = new("Press R to Teleport")
        {
            Visible = false,
        };

        public Teleporter()
        {
            Initialize();
        }

        public Teleporter(string name)
        {
            Name = name;
            Initialize();
        }

        public Teleporter(string name, Vector2 destination)
        {
            Name = name;
            Destination = destination;
            Initialize();
        }

        private void Initialize()
        {
            Info.Position = Position;
        }

        public void Update(Rectangle rec)
        {
            Info.Visible = Intersects(rec);

            if(Info.Visible)
                Info.Position = Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(Rectangle, Color.White, 1, 1);
        }

        public Teleporter Clone()
        {
            return new Teleporter(Name, Destination)
            {
                IsActive = this.IsActive
            };
        }

        public bool Intersects(Rectangle rec)
        {
            return Rectangle.Intersects(rec);
        }

        internal bool CheckRequirements()
        {
            return CheckRequirements(World.Player);
        }

        internal bool CheckRequirements(Player player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player), "Player cannot be null.");
            if (!IsActive)
                return false;
            if (RequiredQuestNames.Count > 0 
                && !RequiredQuestNames.Any(q => player.CompletedQuests.Any(quest => quest.Name == q && quest.IsCompleted)))
                return false;
            return Requirements.CheckRequirements(player);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Teleporter: {Name}");
            sb.AppendLine($"Destination: {Destination}");
            sb.AppendLine($"IsActive: {IsActive}");
            sb.AppendLine($"Position: {Position}");
            sb.AppendLine($"Rectangle: {Rectangle}");
            if (RequiredQuestNames.Count > 0)
                sb.AppendLine($"Required Quests: {string.Join(", ", RequiredQuestNames)}");
            return sb.ToString();
        }

    }
}
