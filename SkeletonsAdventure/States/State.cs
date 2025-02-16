using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonsAdventure.States
{
    public abstract class State
    {
        protected ContentManager Content { get; set; }
        protected GraphicsDevice GraphicsDevice { get; set; }
        protected Game1 Game {  get; set; }
        protected StateManager StateManager { get; set; }
        protected ControlManager ControlManager { get; set; }
        protected PlayerIndex playerIndexInControl;
        
        public SpriteFont menuFont;

        public State(Game1 game)
        {
            playerIndexInControl = PlayerIndex.One;

            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = game.Content;
            StateManager = game.StateManager;

            menuFont = Content.Load<SpriteFont>(@"Fonts\ControlFont");
            ControlManager = new ControlManager(menuFont);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void PostUpdate(GameTime gameTime);
    }
}
