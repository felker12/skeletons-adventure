using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.GameObjects
{
    public class LevelExit(TiledMapObject exit, Level nextLevel)
    {
        public Level NextLevel { get; set; } = nextLevel;
        public Vector2 ExitPosition { get; set; } = new(exit.Position.X, exit.Position.Y);
        public Rectangle ExitArea { get; set; } = new((int)exit.Position.X, 
            (int)exit.Position.Y, (int)exit.Size.Width,(int)exit.Size.Height);
    }
}
