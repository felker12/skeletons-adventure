using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.GameObjects
{
    public class LevelExit(TiledMapObject exit, Level currentLevel, Level nextLevel)
    {
        public TiledMapObject Exit { get; set; } = exit;
        public Level CurrentLevel { get; set; } = currentLevel;
        public Level NextLevel { get; set; } = nextLevel;
        public Vector2 ExitPosition { get; set; } = new(exit.Position.X, exit.Position.Y);
        public Rectangle ExitArea { get; set; } = new((int)exit.Position.X, 
            (int)exit.Position.Y, (int)exit.Size.Width,(int)exit.Size.Height);
    }
}
