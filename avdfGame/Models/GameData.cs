using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avdfGame
{
    public class GameData
    {
        public DateTime SessionDateTime { get; set; }
        public string RoomName { get; set; }
        public string SessionName { get; set; }
        public List<string> PlayerNames;
        public bool IsHost { get; set; }

        public int HostID { get; set; } // not getting set
        public int GameID { get; set; } // not getting set

        public List<Capability> CapabilityData { get; set; }
        public List<PaletteItemData> PaletteItemData { get; set; }
        public Capability PlayerChosenCapability { get; set; }

        public GameData(string gn)
        {
            RoomName = gn;
            PlayerNames = new List<string>();
            CapabilityData = new List<Capability>();
            PaletteItemData = new List<PaletteItemData>();
            PlayerChosenCapability = new Capability();
        }

    }
}
