using Microsoft.Xna.Framework;

namespace SkeletonsAdventure.GameWorld
{
    public class Camera(int screenW, int screenH)
    {
        private int _mapWidth, _mapHeight;

        public Vector2 Position = new();
        public int Width { get; set; } = screenW;
        public int Height { get; set; } = screenH;
        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }

        public void Update(Vector2 playerPosition)
        {
            Position.X = playerPosition.X - (Width / 2);
            Position.Y = playerPosition.Y - (Height / 2);

            //lock the camera to the map
            Position.X = MathHelper.Clamp(Position.X, 0, _mapWidth - Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, _mapHeight - Height);
        }

        public void SetBounds(int width, int height)
        {
            _mapWidth = width;
            _mapHeight = height;
        }
    }
}
