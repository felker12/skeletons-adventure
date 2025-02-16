using Microsoft.Xna.Framework;

namespace SkeletonsAdventure.GameWorld
{
    public class Camera(int screenW, int screenH)
    {
        private Vector2 _cameraPosition = new();
        private readonly int _screenWidth = screenW, _screenHeight = screenH;
        private int _mapWidth, _mapHeight;

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-_cameraPosition, 0f));
            }
        }

        public void Update(Vector2 playerPosition)
        {
            _cameraPosition.X = playerPosition.X - (_screenWidth / 2);
            _cameraPosition.Y = playerPosition.Y - (_screenHeight / 2);

            //lock the camera to the map
            _cameraPosition.X = MathHelper.Clamp(_cameraPosition.X, 0, _mapWidth - _screenWidth);
            _cameraPosition.Y = MathHelper.Clamp(_cameraPosition.Y, 0, _mapHeight - _screenHeight);
        }

        public void SetBounds(int width, int height)
        {
            _mapWidth = width;
            _mapHeight = height;
        }
    }
}
