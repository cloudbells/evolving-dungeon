namespace Evolving_Dungeon
{
    /// <summary>
    /// Enum of different kinds of cell types in a dungeon.
    /// </summary>
    internal enum CellType
    {
        Start, // There is only one start per dungeon.
        Exit, // There is only one exit per dungeon.
        FreeSpace, // These serve as doors between rooms as well as empty spaces inside rooms.
        WallVertical, // Separates the rooms.
        WallHorizontal,
        WallTopLeft,
        WallTopRight,
        WallBottomLeft,
        WallBottomRight,
        Monster, // Should never block a passage between rooms.
        Treasure // Same as above.
    }
}
