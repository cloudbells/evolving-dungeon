namespace Evolving_Dungeon
{
    /// <summary>
    /// A room contained within a dungeon.
    /// </summary>
    internal class Room
    {
        public Cell[] Cells { get; set; }
        public Cell StartCell { get; set; } // Top-left corner of the room.
        public Cell EndCell { get; set; } // Bottom-right corner of the room.
        public bool IsIntact { get; set; }

        /// <summary>
        /// Creates a new room with the given width and height.
        /// </summary>
        /// <param name="width">the width of the room</param>
        /// <param name="height">the height of the room</param>
        /// <param name="cells">an array of all cells contained in the room</param>
        public Room(Cell[] cells)
        {
            Cells = cells;
            StartCell = cells[0];
            EndCell = cells[cells.Length - 1];
        }
    }
}
