using MonoGame.Extended.Tiled;
using RpgLibrary.GameObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.GameObjects
{
    internal class QuestNode : InteractableObject
    {

        public QuestNode(TiledMapObject obj) : base(obj)
        {

        }

        public QuestNode(QuestNode node) : base(node)
        {

        }

        public QuestNode(InteractableObjectData obj) : base(obj) //TODO: change to quest data class if I make one
        {

        }

        public override QuestNode Clone()
        {
            return new(this);
        }
        public override void Interact()
        {
            //TODO
            System.Diagnostics.Debug.WriteLine($"Overrided logic");
            base.Interact();
        }
    }
}
