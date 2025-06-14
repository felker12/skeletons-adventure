using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.Controls
{
    public abstract class Control
    {
        #region Event Region
        public event EventHandler Selected;
        #endregion
        #region Property Region
        public string Name { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Position { get; set; }
        public object Value { get; set; }
        public virtual bool HasFocus { get; set; } = false;
        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public bool TabStop { get; set; }
        public SpriteFont SpriteFont{ get; set; } = GameManager.Arial12;
        public Color TextColor { get; set; } = Color.White;
        public Color BackgroundColor { get; set; } = Color.White;
        public string Type { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
        #endregion
        #region Constructor Region
        public Control()
        {
        }
        public Control(SpriteFont font)
        {
            SpriteFont = font;
        }

        #endregion
        #region Abstract Methods

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput(PlayerIndex playerIndex);
        #endregion
        #region Virtual Methods
        protected virtual void OnSelected(EventArgs e)
        {
            Selected?.Invoke(this, e);
        }
        #endregion
    }
}