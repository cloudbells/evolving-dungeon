namespace Evolving_Dungeon
{
    /// <summary>
    /// A dungeon.
    /// </summary>
    internal class Dungeon : IComparable
    {
        public Cell[] Cells { get; }
        public List<Room> Rooms { get; set; }
        public int Dimension { get; }
        public int Score { get; set; } = 0;

        /// <summary>
        /// Creates a new dungeon.
        /// </summary>
        /// <param name="dimension">the dimension of the dungeon (width = height)</param>
        public Dungeon(int dimension)
        {
            Dimension = dimension;
            Cells = new Cell[dimension * dimension];
            for (int i = 0; i < Cells.Length; i++) {
                Cells[i] = new Cell(CellType.FreeSpace, i / Dimension, i % Dimension);
            }
            Rooms = new List<Room>();
        }

        /// <summary>
        /// Create a new dungeon from an old one.
        /// </summary>
        /// <param name="oldDungeon">the dungeon to copy</param>
        public Dungeon(Dungeon oldDungeon)
        {
            Cell[] oldCells = oldDungeon.Cells;
            List<Room> oldRooms = oldDungeon.Rooms;
            Dimension = oldDungeon.Dimension;
            // Deep copy cells.
            Cells = new Cell[oldCells.Length];
            for (int i = 0; i < oldCells.Length; i++)
            {
                Cells[i] = new Cell(oldCells[i].Type, oldCells[i].Row, oldCells[i].Col);
                Cells[i].IsImmune = oldCells[i].IsImmune;
            }
            // Deep copy rooms.
            Rooms = new List<Room>();
            foreach (Room oldRoom in oldRooms)
            {
                Cell[] oldRoomCells = oldRoom.Cells;
                Cell[] newRoomCells = new Cell[oldRoomCells.Length];
                for (int i = 0; i < oldRoomCells.Length; i++)
                {
                    newRoomCells[i] = GetCell(oldRoomCells[i].Row, oldRoomCells[i].Col);
                }
                Room newRoom = new Room(newRoomCells);
                newRoom.IsIntact = oldRoom.IsIntact;
                Rooms.Add(newRoom);
            }
        }

        /// <summary>
        /// Checks if the given room in this dungeon is intact. Returns true if it is.
        /// </summary>
        /// <param name="room">the room to check</param>
        /// <returns>true if the given room is intact, false otherwise</returns>
        public bool CheckRoomIsIntact(Room room)
        {
            room.IsIntact = false;
            Cell startCell = room.StartCell;
            Cell endCell = room.EndCell;
            // Check corners.
            if (startCell.Type != CellType.WallTopLeft
                || GetCellType(startCell.Row, endCell.Col) != CellType.WallTopRight
                || GetCellType(endCell.Row, startCell.Col) != CellType.WallBottomLeft
                || endCell.Type != CellType.WallBottomRight)
            {
                return false;
            }
            // Check horizontal walls.
            for (int col = startCell.Col + 1; col < endCell.Col; col++)
            {
                if (GetCellType(startCell.Row, col) != CellType.WallHorizontal
                    || GetCellType(endCell.Row, col) != CellType.WallHorizontal)
                {
                    return false;
                }
            }
            // Check vertical walls.
            for (int row = startCell.Row + 1; row < endCell.Row; row++)
            {
                if (GetCellType(row, startCell.Col) != CellType.WallVertical
                    || GetCellType(row, endCell.Col) != CellType.WallVertical)
                {
                    return false;
                }
            }
            room.IsIntact = true;
            foreach (Cell cell in room.Cells)
            {
                cell.IsImmune = true;
            }
            return true;
        }

        /// <summary>
        /// Adds a new Room to this dungeon. Returns true if the addition was successful.
        /// </summary>
        /// <param name="startCell">the top-left cell of the room</param>
        /// <param name="endCell">the bottom-right cell of the room</param>
        /// <returns>true if room was added successfully, false otherwise</returns>
        public bool AddRoom(Cell startCell, Cell endCell)
        {
            LinkedList<Cell> cells = new LinkedList<Cell>();
            for (int row = startCell.Row; row <= endCell.Row; row++)
            {
                for (int col = startCell.Col; col <= endCell.Col; col++)
                {
                    Cell cell = GetCell(row, col);
                    if (cell.IsImmune) // If it's already part of a finished room, don't mess with it and return.
                    {
                        return false;
                    }
                    cell.Type = CellType.FreeSpace;
                    cells.AddLast(cell);
                }
            }
            startCell.Type = CellType.WallTopLeft; // Top left.
            SetCellType(startCell.Row, endCell.Col, CellType.WallTopRight); // Top right.
            SetCellType(endCell.Row, startCell.Col, CellType.WallBottomLeft); // Bottom left.
            endCell.Type = CellType.WallBottomRight; // Bottom right.
            // Add horizontal walls.
            for (int col = startCell.Col + 1; col < endCell.Col; col++)
            {
                SetCellType(startCell.Row, col, CellType.WallHorizontal); // Upper.
                SetCellType(endCell.Row, col, CellType.WallHorizontal); // Lower.
            }
            // Add vertical walls.
            for (int row = startCell.Row + 1; row < endCell.Row; row++)
            {
                SetCellType(row, startCell.Col, CellType.WallVertical); // Left.
                SetCellType(row, endCell.Col, CellType.WallVertical); // Right.
            }
            Rooms.Add(new Room(cells.ToArray()));
            return true;
        }

        /// <summary>
        /// Removes the given room from the dungeon.
        /// </summary>
        /// <param name="room">the room to remove</param>
        public void RemoveRoom(Room room)
        {
            Rooms.Remove(room);
        }

        /// <summary>
        /// Returns the cell at the given row and column.
        /// </summary>
        /// <param name="row">the row of the cell</param>
        /// <param name="col">the column of the cell</param>
        /// <returns>the cell at the given row and column</returns>
        public Cell GetCell(int row, int col)
        {
            return Cells[row * Dimension + col];
        }

        /// <summary>
        /// Sets the cell type of the cell at the given row and column.
        /// </summary>
        /// <param name="row">the row of the cell</param>
        /// <param name="col">the column of the cell</param>
        /// <param name="type">the new cell type</param>
        public void SetCellType(int row, int col, CellType type)
        {
            Cells[row * Dimension + col].Type = type;
        }

        /// <summary>
        /// Returns the cell type of the cell at the given row and column.
        /// </summary>
        /// <param name="row">the row of the cell</param>
        /// <param name="col">the column of the cell</param>
        /// <returns>the cell type of the cell</returns>
        public CellType GetCellType(int row, int col)
        {
            return GetCell(row, col).Type;
        }

        /// <summary>
        /// Prints the dungeon for debug.
        /// </summary>
        public void Print()
        {
            int count = 0;
            foreach (Cell cell in Cells)
            {
                if (cell.IsImmune)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                Console.Write(cell);
                Console.BackgroundColor = ConsoleColor.Black;
                count++;
                if (count % Dimension == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        public override string ToString()
        {
            string res = "";
            int count = 0;
            foreach (Cell cell in Cells)
            {
                res += cell;
                count++;
                if (count % Dimension == 0)
                {
                    res += "\n";
                }
            }
            return res;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }
            Dungeon? dungeon2 = obj as Dungeon;
            if (dungeon2 != null)
            {
                return Score.CompareTo(dungeon2.Score);
            }
            return 1;
        }
    }
}
