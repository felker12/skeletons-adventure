using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkeletonsAdventure.Controls;

namespace SkeletonsAdventure.GameMenu
{
    internal class PlayerInfoMenu : BaseMenu
    {
        // This menu is used to display player information such as stats, and such.
        public Player Player { get; private set; } = World.Player;

        public PlayerInfoMenu() : base()
        {
            CreateControls();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Position the controls
            AttributePoints.Position = new(Position.X + 10, Position.Y + 10);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void CreateControls()
        {
            AttributePoints = new Label
            {
                SpriteFont = GameManager.Arial14,
            };

            ControlManager.Add(AttributePoints);
        }

        public void UpdateWithPlayer(Player player)
        {
            Player = player;

            AttributePoints.Text = "Attribute Points: " + Player.AttributePoints.ToString();
        }

        //Controls for displaying player information can be added here
        Label AttributePoints { get; set; }
    }
}
