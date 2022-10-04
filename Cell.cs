namespace Evolving_Dungeon
{
    /// <summary>
    /// A cell within a dungeon.
    /// </summary>
    internal class Cell
    {
        public CellType Type { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsImmune { get; set; } // True if immune to removal.

        /// <summary>
        /// Maps cell type to its string representation.
        /// </summary>
        private static Dictionary<CellType, string> toChar = new Dictionary<CellType, string>()
        {
            {CellType.Start, "S"},
            {CellType.Exit, "E"},
            {CellType.FreeSpace, " "},
            {CellType.WallVertical, "║"},
            {CellType.WallHorizontal, "═"},
            {CellType.WallTopLeft, "╔"},
            {CellType.WallTopRight, "╗"},
            {CellType.WallBottomLeft, "╚"},
            {CellType.WallBottomRight, "╝"},
            {CellType.Monster, "M"},
            {CellType.Treasure, "T"},
        };

        /// <summary>
        /// Create a new cell with the given type, and at the given row and column.
        /// </summary>
        /// <param name="type">the type of the cell</param>
        /// <param name="row">the row of the cell</param>
        /// <param name="col">the column of the cell</param>
        public Cell(CellType type, int row, int col)
        {
            Type = type;
            Row = row;
            Col = col;
        }

        public override string ToString()
        {
            return toChar[Type];
        }
    }
}
