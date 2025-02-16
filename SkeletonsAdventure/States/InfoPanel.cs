using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SkeletonsAdventure.ItemClasses;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace SkeletonsAdventure.States
{
    public class InfoPanel
    {
        public List<GameItem> Items { get; set; }
        public Viewport Viewport { get; set; }
        private readonly Texture2D _texture;
        private Rectangle _viewportRec;
        private readonly TiledMapRenderer _tiledMapRenderer;

        public InfoPanel(Viewport viewport, List<GameItem> items, GraphicsDevice graphicsDevice, TiledMap backsplash)
        {
            Items = items;
            Viewport = viewport;
            _texture = new(graphicsDevice, 1, 1);
            _texture.SetData([Color.DarkSlateGray]);
            _viewportRec = new(0, 0, Viewport.Width, Viewport.Height);
            _tiledMapRenderer = new(graphicsDevice);
            _tiledMapRenderer.LoadMap(backsplash);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _viewportRec, Color.White);
            spriteBatch.DrawRectangle(_viewportRec, Color.Black, 2, 0);
            spriteBatch.End();

            _tiledMapRenderer.Draw();

            spriteBatch.Begin();
            //Draw the backback items
            foreach (GameItem item in Items)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public void Update(List<GameItem> items)
        {
            Items = items;

            //19 colums 4 rows
            int[,] inventory = new int[19,4];
            Vector2 pos = new(32, 32);

            int itmCnt = 0;
            for (int i = 0; i < inventory.GetLength(0); i++)
            {
                for (int j = 0; j < inventory.GetLength(1); j++)
                {
                    if (itmCnt < Items.Count)
                    {
                        pos.X = (j + 1) * 32;
                        pos.Y = (i + 1) * 32;
                        Items[itmCnt].Position = pos;
                        Items[itmCnt].ItemRectangle = new Rectangle((int)pos.X, (int)pos.Y, 32, 32);

                        itmCnt++;
                    }
                    else break;
                }
            }
        }
    }
}
