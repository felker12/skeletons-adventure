using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;
using MonoGame.Extended;

namespace SkeletonsAdventure.Entities
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Label Info { get; set; }
        public Rectangle GetRectangle => new((int)Position.X, (int)Position.Y, Width, Height);
        public Rectangle Frame { get; set; }
        public Color DefaultColor { get; set; } = Color.White;
        public Color SpriteColor { get; set; }
        public Vector2 Position { get; set; } = new();
        public Vector2 Motion { get; set; } = new();
        public float Speed { get; set; } = 1.5f;
        public int Width { get; set; } = 32;
        public int Height { get; set; } = 32;
        public float RotationAngle { get; set; } = 0.0f;
        public float Scale { get; set; } = 1.0f;
        //TODO make use of the CanMove property. For example when the entity is stunned or frozen or casting a spell
        public bool CanMove { get; set; } = true; 

        public Sprite()
        {
            Texture = GameManager.SkeletonTexture; //default to the player/skeleton sprite
            Initialize();
        }

        private void Initialize()
        {
            SpriteColor = DefaultColor;
            Info = new()
            {
                Text = "",
                Position = new Vector2(Position.X, Position.Y),
                Color = Color.White,
                SpriteFont = GameManager.Arial12
            };

            Frame = new(0, 0, Width, Height);
        }

        public virtual void Update(GameTime gameTime)
        {
            Position = LockToMap(Position);
            Info.Text = string.Empty; //Set the text back to empty before adding something to it (like from a child class)
            Info.Position = LockToMap(Position - new Vector2(0, 100));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 FrameCenter = new(Frame.Width / 2, Frame.Height / 2);

            spriteBatch.Draw(Texture, Position + FrameCenter, Frame, SpriteColor, RotationAngle, FrameCenter, Scale, SpriteEffects.None, 1);
            //spriteBatch.Draw(Texture, Position, Frame, SpriteColor);

            //spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO

            Info.Draw(spriteBatch);
        }

        public Vector2 LockToMap(Vector2 position)
        {
            return new(MathHelper.Clamp(position.X, 0, World.CurrentLevel.Width - Width),
                MathHelper.Clamp(position.Y, 0, World.CurrentLevel.Height - Height));
        }

        public Vector2 GetCenter()
        {
            return Position + new Vector2(Width / 2, Height / 2);
        }
    }
}
