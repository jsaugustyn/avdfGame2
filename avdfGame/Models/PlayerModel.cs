using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.ComponentModel;

namespace avdfGame
{
    public class PlayerModel { }

    public class Player
    {
        public string UserName { get; set; }
        public bool IsHost { get; set; }
        public int UserId { get; set; }
        //public NetworkModel UserNetwork;

        //public MainWindow PlayerMainWindow { get; set; }

        private string fname;

        public Player()
        {
        }

        // METHODS

        public void WriteToLog(string msg)
        {
            if(fname==null)
            {
                // create the log file if it hasn't been initialized yet
                fname = UserName + "-output.txt";
                using (StreamWriter writer = new StreamWriter(fname))
                {
                    writer.WriteLine(String.Format("Player {0} created", UserName));
                }
            }

            using (StreamWriter writer = new StreamWriter(fname, true))
            {
                writer.WriteLine(msg);
            }
        }
    }
}
