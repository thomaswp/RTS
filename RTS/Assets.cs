using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Player;

namespace RTS
{
    public static class Assets
    {
        public static string AssetPath { get; set; }
        public static string GraphicsPath { get { return AssetPath + @"Graphics\"; } }

        public static Bitmap LoadBuilding(string path)
        {
            return new Bitmap(GraphicsPath + @"Buildings\" + path);
        }

        public static Bitmap LoadTile(string path)
        {
            return new Bitmap(GraphicsPath + @"Tiles\" + path);
        }
    }
}
