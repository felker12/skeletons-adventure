using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.GameMenu
{
    internal class PlayerInfoMenu : BaseMenu
    {
        // This menu is used to display player information such as stats, and such.
        PlayerData PlayerData { get; set; } = new();


        public event EventHandler MenuOpened; //TODO when the menu is opened, this event should be raised to update the player data
        public PlayerInfoMenu() : base()
        {

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
