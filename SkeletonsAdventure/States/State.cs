using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.States
{
    public abstract class State
    {
        protected ContentManager Content { get; set; }
        protected GraphicsDevice GraphicsDevice { get; set; }
        protected Game1 Game {  get; set; }
        protected StateManager StateManager { get; set; }
        public SpriteFont MenuFont { get; set; }
        protected ControlManager ControlManager { get; set; }
        protected PlayerIndex PlayerIndexInControl { get; set; }

        public State(Game1 game)
        {
            PlayerIndexInControl = PlayerIndex.One;

            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = game.Content;
            StateManager = game.StateManager;

            MenuFont = GameManager.Arial20;
            ControlManager = new ControlManager(MenuFont);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void PostUpdate(GameTime gameTime);
    }
}
