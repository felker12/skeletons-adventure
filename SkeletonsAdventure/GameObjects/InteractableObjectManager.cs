using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.GameObjects
{
    internal class InteractableObjectManager
    {
        private List<InteractableObject> InteractableObjects { get; set; } = [];

        public InteractableObjectManager() { }

        public void Update(GameTime gameTime, Rectangle player)
        {
            foreach (var obj in InteractableObjects)
            {
                obj.Update(gameTime, player);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var obj in InteractableObjects)
            {
                obj.Draw(spriteBatch);
            }
        }

        public void Add(InteractableObject obj)
        {
            InteractableObjects.Add(obj);
        }
        public void Remove(InteractableObject obj)
        {
            InteractableObjects.Remove(obj);
        }
    }
}
