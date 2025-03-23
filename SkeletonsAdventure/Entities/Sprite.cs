using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.Entities
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
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
            Texture = GameManager.SkeletonTexture; //default to the player/skeleton sprite
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
            Position = LockToMap(Position);
            Info.Position = LockToMap(Position - new Vector2(0, 60));

            GetRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            if (IsColliding)
                SpriteColor = Color.Red;
            else
                SpriteColor = DefaultColor;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Frame, SpriteColor);
            //spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO
            Info.Draw(spriteBatch);
        }

        public Vector2 LockToMap(Vector2 position)
        {
            return new()
            {
                X = MathHelper.Clamp(position.X, 0, World.CurrentLevel.Width - Width),
                Y = MathHelper.Clamp(position.Y, 0, World.CurrentLevel.Height - Height)
            };
        }
    }
}
