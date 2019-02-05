using System;
using GlLib.Graphic;

namespace GlLib.Map
{
    public class AutumnGrassWithStone : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.autumn.stone";
        }

        public override Texture GetTexture(int x, int y)
        {
            return Vertexer.LoadTexture("grass_autumn_st_" + (Math.Abs(Math.Round(Math.Cos(x)+Math.Sin(y)) % 2)) + ".png");
        }
    }
}