using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.Entities
{
    public class Sprite
    {
        public Texture2D texture;
        public float speed;
        public bool isCollidingBoundary;

        public Label Info { get; set; }
        public Rectangle GetRectangle { get; set; }
        public Rectangle Frame { get; set; }
        public Color SpriteColor { get; set; }
        public Color DefaultColor { get; set; }
        public Vector2 Position { get; set; } = new();
        public Vector2 Motion { get; set; } = new();
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsColliding { get; set; }
        public int CollisionCount { get; set; }

        public Sprite()
        {
            texture = GameManager.SkeletonTexture; //default to the player/skeleton sprite
            Initialize();
            Info.SpriteFont = GameManager.InfoFont;
        }

        private void Initialize()
        {
            IsColliding = false;
            isCollidingBoundary = false;
            DefaultColor = Color.White;
            SpriteColor = DefaultColor;
            Width = 32;
            Height = 32;
            CollisionCount = 0;
            speed = 1.5f;
            Info = new()
            {
                Text = "",
                Position = new Vector2(Position.X, Position.Y),
                Color = Color.White
            };
        }

        public virtual void Update(GameTime gameTime)
        {
            LockToMap();
            Info.Position = new Vector2(MathHelper.Clamp(Position.X, 0, 1000000000),
                MathHelper.Clamp(Position.Y - 60, 0, 1000000000));
            GetRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            if (IsColliding)
                SpriteColor = Color.Red;
            else
                SpriteColor = DefaultColor;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Frame, SpriteColor);
            //spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO
            Info.Draw(spriteBatch);
        }

        public void LockToMap()
        {
            //TODO set the max value based on the width of the map
            Vector2 pos = new()
            {
                X = MathHelper.Clamp(Position.X, 0, 1000000000),
                Y = MathHelper.Clamp(Position.Y, 0, 1000000000)
            };
            Position = pos;
        }
    }
}
