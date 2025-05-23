using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.Engines;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.Quests;

namespace SkeletonsAdventure.GameObjects
{
    internal class InteractableObject : AnimatedSprite
    {
        public string TypeOfObject { get; set; } = string.Empty;

        public InteractableObject(TiledMapObject obj) : base() 
        {
            Position = new Vector2(obj.Position.X, obj.Position.Y);
            Width = (int)obj.Size.Width;
            Height = (int)obj.Size.Height;

            if (obj.Properties.TryGetValue("TypeOfObject", out TiledMapPropertyValue value))
                TypeOfObject = value.ToString();

            Info.Position = Position;
            Info.Text = "Press R to Interact";
            Info.Visible = false;
            Info.Color = Color.Orange;
        }

        public InteractableObject(InteractableObject obj) : base()
        {
            TypeOfObject = obj.TypeOfObject;
            Info = obj.Info;

            //Info.Position = Position;
            //Info.Text = "Press R to Interact";
            //Info.Visible = false;
            //Info.Color = Color.Orange;
        }

        public void Update(GameTime gameTime, Rectangle player)
        {
            //base.Update(gameTime);

            CheckPlayerNear(player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);

            if(Info.Visible)
                Info.Draw(spriteBatch);

            spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO
        }

        public InteractableObject Clone()
        {
            return new InteractableObject(this);
        }

        public InteractableObjectData GetInteractableObjectData()
        {
            return new InteractableObjectData()
            {
                TypeOfObject = TypeOfObject,
                Position = Position,
                Width = Width,
                Height = Height
            };
        }

        public void CheckPlayerNear(Rectangle player)
        {
            if(GetRectangle.Intersects(player))
            {
                Info.Visible = true;

                if (InputHandler.KeyReleased(Keys.R) ||
                    InputHandler.ButtonDown(Buttons.A, PlayerIndex.One))
                {
                    //TODO add the interaction logic here
                    System.Diagnostics.Debug.WriteLine($"Interacting with {TypeOfObject} at {Position}");
                }
            }
            else
            {
                Info.Visible = false;
            }
        }
    }
}
