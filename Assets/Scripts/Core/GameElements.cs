namespace GeminiCore
{
    // Enum to define different types of tiles on the grid
    public enum TileType
    {
        Empty,
        SoftEarth,
        SolidRock,
        PhaseGravel,
        AetherShard,
        CorruptedMineral,
        ExitPortal
    }

    // Represents a collectible Aether Shard
    public class AetherShard
    {
        public int x;
        public int y;
    }

    // Represents a block of Phase Gravel
    public class PhaseGravel
    {
        public int x;
        public int int y;
        public bool isSolid;
    }

    // Represents a falling rock or gem
    public class CorruptedMineral
    {
        public int x;
        public int y;
    }

    // Represents a Plasma Wasp enemy
    public class PlasmaWasp
    {
        public int x;
        public int y;
    }
}
