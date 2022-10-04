namespace Evolving_Dungeon
{
    /// <summary>
    /// A generator which creates randomized dungeons.
    /// </summary>
    internal class DungeonGenerator
    {
        private static readonly int NBR_OF_DUNGEONS = 10;
        private int lambda; // This many copies will be made between generations.
        private Dungeon[] dungeons;
        private Random random = new Random(Environment.TickCount);

        /// <summary>
        /// Creates a new dungeon generator.
        /// </summary>
        public DungeonGenerator()
        {
            dungeons = new Dungeon[NBR_OF_DUNGEONS];
        }

        /// <summary>
        /// Generates a random dungeon with the given dimension and returns it.
        /// </summary>
        /// <param name="dimension">the dimension (width = height) of the dungeon – must be at least 10</param>
        /// <returns>the generated dungeon</returns>
        /// <exception cref="DimensionSizeException">if the given dimension is less than 10</exception>
        public Dungeon Generate(int dimension, int nbrOfRooms)
        {
            // Each room will take a minimum of 9 cells (which leaves only one free space in the room).
            int minDimension = (int)Math.Ceiling(Math.Sqrt(nbrOfRooms * 9));
            if (dimension < minDimension)
            {
                throw new DimensionSizeException("Dungeon dimension must be at least " + minDimension + " (min total size is 9 * nbrOfRooms)");
            }
            lambda = NBR_OF_DUNGEONS / 2;
            // Initialize the population.
            for (int i = 0; i < NBR_OF_DUNGEONS; i++)
            {
                dungeons[i] = new Dungeon(dimension);
                // Place rooms.
                for (int j = 0; j < nbrOfRooms; j++)
                {
                    AddRoom(dungeons[i]);
                }
            }
            Dungeon best;
            int counter = 1;
            do
            {
                Console.WriteLine("---Iteration number " + counter++ + "---");
                EvolveDungeons();
                best = EvaluateDungeons();
            } while (best.Score < nbrOfRooms * 10);
            return best;
        }

        /// <summary>
        /// Evaluates all dungeons.
        /// </summary>
        /// <returns>the best dungeon for the current generation</returns>
        private Dungeon EvaluateDungeons()
        {
            Dungeon best = dungeons[0];
            foreach (Dungeon dungeon in dungeons)
            {
                int score = 0;
                // For each room that's intact, add 10 points.
                foreach (Room room in dungeon.Rooms)
                {
                    if (dungeon.CheckRoomIsIntact(room))
                    {
                        score += 10;
                    }
                }
                dungeon.Score = score;
                if (score > best.Score)
                {
                    best = dungeon;
                }
            }
            // Sort the array and remove the (lambda) worst individuals, replace them with copies of the best individuals.
            Array.Sort(dungeons);
            Console.WriteLine("Best dungeon found in this generation:");
            dungeons[dungeons.Length - 1].Print();
            Console.WriteLine("Worst dungeon found in this generation:");
            dungeons[0].Print();
            for (int i = 0; i < lambda; i++)
            {
                dungeons[i] = new Dungeon(dungeons[NBR_OF_DUNGEONS - lambda + i]);
            }
            return best;
        }

        /// <summary>
        /// Evolves all dungeons.
        /// </summary>
        private void EvolveDungeons()
        {
            foreach (Dungeon dungeon in dungeons)
            {
                // First, remove all cells that aren't part of a room.
                foreach (Cell cell in dungeon.Cells)
                {
                    if (!cell.IsImmune && cell.Type != CellType.FreeSpace)
                    {
                        cell.Type = CellType.FreeSpace;
                    }
                }
                // Then, add new random rooms (without touching the existing rooms).
                foreach (Room room in dungeon.Rooms.ToList())
                {
                    if (!room.IsIntact)
                    {
                        dungeon.RemoveRoom(room);
                        bool wasSuccess;
                        do
                        {
                            wasSuccess = AddRoom(dungeon);
                        } while (!wasSuccess);
                    }
                }
                //foreach (Cell cell in dungeon.Cells) // No cells are immune now. // TODO: can i remove this?
                //{
                //    cell.IsImmune = false;
                //}
            }
        }

        /// <summary>
        /// Adds a new Room to the given dungeon. Returns true if the addition was successful.
        /// </summary>
        /// <param name="dungeon">the dungeon to add the room to</param>
        /// <returns>true if room was added successfully, false otherwise</returns>
        private bool AddRoom(Dungeon dungeon)
        {
            int dimension = dungeon.Dimension;
            int width = random.Next(3, Math.Max(dimension / 4, 3));
            int height = random.Next(3, Math.Max(dimension / 4, 3));
            int startRow = random.Next(0, dimension - width);
            int startCol = random.Next(0, dimension - height);
            int endRow = startRow + width;
            int endCol = startCol + height;
            Cell startCell = dungeon.GetCell(startRow, startCol);
            Cell endCell = dungeon.GetCell(endRow, endCol);
            return dungeon.AddRoom(startCell, endCell);
        }

        /// <summary>
        /// Adds a random entrance or exit to the given dungeon.
        /// </summary>
        /// <param name="dungeon">the dungeon to add the door to</param>
        /// <param name="isEntrance">true if the door should be an entrance, false if exit</param>
        private void AddDoor(Dungeon dungeon, bool isEntrance)
        {
            int dimension = dungeon.Dimension;
            int wall = random.Next(0, 3);
            int index = random.Next(1, dimension - 2);
            int row = 0;
            int col = 0;
            CellType cellType = isEntrance ? CellType.Start : CellType.Exit;
            switch (wall)
            {
                case 0: // Top wall.
                    row = 0;
                    col = index;
                    break;
                case 1: // Right wall.
                    row = index;
                    col = dimension - 1;
                    break;
                case 2: // Bottom wall.
                    row = dimension - 1;
                    col = index;
                    break;
                case 3: // Left wall.
                    row = index;
                    col = 0;
                    break;
            }
            CellType current = dungeon.GetCellType(row, col);
            if (current == CellType.Start || current == CellType.Exit) // If the space is already occupied, redo.
            {
                AddDoor(dungeon, isEntrance);
            }
            else
            {
                dungeon.SetCellType(row, col, cellType);
            }
        }
    }
}
