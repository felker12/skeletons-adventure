using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.MenuClasses;
using SkeletonsAdventure.Controls;
using System.Collections.Generic;
namespace SkeletonsAdventure.GameMenu
{
    internal class TabbedMenu : BaseMenu
    {
        public TabBar TabBar { get; set; } = new();

        public TabbedMenu() : base()
        {
            Position = new Vector2(0,0);
            Width = 600;
            Height = 500;

            TabBar.Width = Width;
            TabBar.Height = TabBar.MaxTabHeight();
            TabBar.Position = Position;
        }

        public TabbedMenu(TabbedMenuData tabbedMenuData) : base(tabbedMenuData)
        {
            BaseMenu activeMenu = null;

            foreach(MenuData menuData in tabbedMenuData.MenuDatas)
            {
                TabBar.AddMenu(new BaseMenu(menuData));
            }

            foreach(BaseMenu menu in TabBar.TabMenus.Values)
            {
                if(menu.Title == tabbedMenuData.ActiveMenu)
                    activeMenu = menu;
            }

            TabBar.Position = Position;
            TabBar.Width = Width;
            TabBar.Height = TabBar.MaxTabHeight();
            TabBar.SetActiveTab(activeMenu);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Visible)
            {
                TabBar.Update();
                TabBar.Position = Position;

                if (TabBar.ActiveMenu != null)
                {
                    TabBar.ActiveMenu.Position = TabBar.Position + new Vector2(0, TabBar.Height);
                    TabBar.ActiveMenu.Update(gameTime);
                    TabBar.ActiveMenu.Width = Width;
                    TabBar.ActiveMenu.Height = Height - TabBar.Height;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Texture, Rectangle, TintColor);
                TabBar.Draw(spriteBatch);
                ControlManager.Draw(spriteBatch);
                spriteBatch.End();

                TabBar.ActiveMenu?.Draw(spriteBatch);
            }
        }

        public void SetMenuData(TabbedMenuData tabbedMenuData)
        {
            base.SetMenuData(tabbedMenuData);
            BaseMenu activeMenu = null;

            foreach (MenuData menuData in tabbedMenuData.MenuDatas)
            {
                foreach (BaseMenu menu in TabBar.TabMenus.Values)
                {
                    if (menu.Title == tabbedMenuData.ActiveMenu)
                        activeMenu = menu;

                    if(menuData.Title == menu.Title)
                        menu.SetMenuData(menuData);
                }
            }

            TabBar.Width = Width;
            TabBar.Height = TabBar.MaxTabHeight();
            TabBar.SetActiveTab(activeMenu);
        }

        public void AddMenu(BaseMenu menu)
        {
            menu.Position = TabBar.Position + new Vector2(0, TabBar.Height);
            TabBar.AddMenu(menu);
        }

        public TabbedMenuData GetTabbedMenuData()
        {
            List<MenuData> menuDatas = [];
            foreach(BaseMenu menu in TabBar.TabMenus.Values)
            {
                menuDatas.Add(menu.GetMenuData());
            }

            return new TabbedMenuData()
            {
                Visible = Visible,
                Title = Title,
                Width = Width,
                Height = Height,
                Position = Position,
                TintColor = TintColor,
                BackgroundColor = BackgroundColor,
                ActiveMenu = TabBar.ActiveMenu?.Title ?? string.Empty,
                MenuDatas = menuDatas
            };
        }
    }
}
