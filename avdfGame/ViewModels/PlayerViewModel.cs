using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avdfGame.ViewModels
{
    public class PlayerViewModel
    {
        private Player player;
        public Player Player
        {
            get { return player; }
            set
            {
                if(player==null)
                {
                    player = new Player();
                    player = value;
                }
            }
        }

        public PlayerViewModel()
        {
            player = new Player();
        }


    }
}
