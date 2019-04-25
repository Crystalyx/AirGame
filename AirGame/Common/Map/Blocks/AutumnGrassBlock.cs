namespace GlLib.Common.Map.Blocks
{
    public class AutumnGrassBlock : TerrainBlock
    {
        public override string GetName()
        {
            return "block.outdoor.grass.autumn";
        }

        public override string GetTextureName(World _world, int _x, int _y)
        {
            return "grass_autumn.png";
        }
    }
}