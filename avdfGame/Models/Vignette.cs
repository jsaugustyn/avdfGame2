using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace avdfGame
{
    public class Vignette
    {
        public Mission MissionType { get; set; }
        public List<EnemyChar> EnemyCharacteristics { get; set; }
        public List<Terrain> TerrainCharacteristics { get; set; }
        public Troops TroopType { get; set; }
        public TimeData Time { get; set; } 
        public List<Civil> CivilCharacteristics { get; set; }

        public BitmapImage VignetteImage { get; set; }

        public Vignette()
        {
            MissionType = new Mission();
            EnemyCharacteristics = new List<EnemyChar>();
            TerrainCharacteristics = new List<Terrain>();
            TroopType = new Troops();
            Time = new TimeData();
            CivilCharacteristics = new List<Civil>();
            VignetteImage = new BitmapImage();
        }
    }

    public class VignetteData
    {
        public string AttributeType { get; set; }
        public string AttributeName { get; set; }
        public string AttributeDetail { get; set; }
    }

    public class Mission
    {
        public String MissionTitle { get; set; }
        public String MissionText { get; set; }
    }

    public class EnemyChar
    {
        public String EnemyCharTitle { get; set; }
        public String EnemyCharText { get; set; }
    }

    public class Terrain
    {
        public String TerrainCharTitle { get; set; }
        public String TerrainCharText { get; set; }
    }

    public class Troops
    {
        public String TroopsTitle { get; set; }
        public String TroopsText { get; set; }
    }

    public class TimeData
    {
        public String TimeTitle { get; set; }
        public String TimeText { get; set; }
    }

    public class Civil
    {
        public String CivilCharTitle { get; set; }
        public String CivilCharText { get; set; }
    }
}
