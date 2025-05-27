using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.Engines;
using SkeletonsAdventure.Entities;

namespace SkeletonsAdventure.GameObjects
{
    internal class InteractableObject : AnimatedSprite
    {
        public string TypeOfObject { get; set; } = string.Empty;
        public bool Active { get; set; } = true;

        public InteractableObject(TiledMapObject obj) : base()
        {
            Position = new Vector2(obj.Position.X, obj.Position.Y);
            Width = (int)obj.Size.Width;
            Height = (int)obj.Size.Height;

            if (obj.Properties.TryGetValue("TypeOfObject", out TiledMapPropertyValue value))
                TypeOfObject = value.ToString();

            Initialize();
        }

        public InteractableObject(InteractableObject obj) : base()
        {
            TypeOfObject = obj.TypeOfObject;
            Info = obj.Info;
            Active = obj.Active;
            Width = obj.Width;
            Height = obj.Height;
            Position = obj.Position;

            //Info.Position = Position;
            //Info.Text = "Press R to Interact";
            //Info.Visible = false;
            //Info.Color = Color.Orange;
        }

        public InteractableObject(InteractableObjectData obj)
        {
            TypeOfObject = obj.TypeOfObject;
            Active = obj.Active;
            Width = obj.Width;
            Height = obj.Height;
            Position = obj.Position;

            Initialize();
        }

        private void Initialize()
        {
            Info.Position = Position;
            Info.Text = "Press R to Interact";
            Info.Visible = false;
            Info.Color = Color.Orange;
        }

        public virtual void Update(GameTime gameTime, Player player)
        {
            //base.Update(gameTime);

            if (CheckPlayerNear(player))
            {
                HandleInput(player);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);

            if (Info.Visible)
                Info.Draw(spriteBatch);

            spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO
        }

        public virtual InteractableObject Clone()
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
                Height = Height,
                Active = Active,
            };
        }

        public bool CheckPlayerNear(Player player)
        {
            if(Active)
            {
                if (GetRectangle.Intersects(player.GetRectangle))
                    Info.Visible = true;
                else
                    Info.Visible = false;
            }

            return Info.Visible;
        }

        public virtual void HandleInput(Player player)
        {
            // This method can be overridden in derived classes to handle specific input logic
            if (InputHandler.KeyReleased(Keys.R) || InputHandler.ButtonDown(Buttons.A, PlayerIndex.One))
            {
                Interact(player);
            }
        }

        public virtual void Interact(Player player)
        {
            // This method can be overridden in derived classes to provide specific interaction logic
            System.Diagnostics.Debug.WriteLine($"Interacting with {TypeOfObject} at {Position}" +
                $", of type {this.GetType().Name}");
        }
    }
}
