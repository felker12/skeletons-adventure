using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.GameMenu
{
    internal class QuestMenu : BaseMenu
    {
        //TODO this menu is used to display the player's quests
        public Player Player { get; set; } = World.Player;

        public QuestMenu() : base()
        {

        }

        public void SetPlayer(Player player)
        {
            Player = player;
        }
    }
}
