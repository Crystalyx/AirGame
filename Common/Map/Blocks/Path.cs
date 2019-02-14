using GlLib.Client.Graphic;

namespace GlLib.Common.Map.Blocks
{
    public class Path : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.path";
        }

        public override string GetTextureName(World world,int x, int y)
        {
            return "path.png";
        }
    }
}