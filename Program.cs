namespace Evolving_Dungeon
{
    /// <summary>
    /// Main entry point.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {


            // TODO: it will get stuck if the rooms are too big, new rooms wont be able to fit so it will be an endless loop
            // TODO: should be able to specify number of rooms, so change exception to be logic and not hard coded
            // TODO: debug why it gets stuck

            // report: drawback is that if two dungeons have the same score one might be "better" but there is no way to distinguish them


            /* 1. We start by producing a single square dungeon.
             * 2. We use a simple evolutionary algorithm to generate rooms in the dungeon.
             * 3. Once the dungeon has been evaluated, if it's 'good enough', we add corridors/treasure/monsters and we are done.
             * 
             * The evolutionary algorithm isn't ideal for building dungeons. A better approach would be a constructive algorithm.
             */





            DungeonGenerator generator = new DungeonGenerator();
            Dungeon dungeon = generator.Generate(20, 10);
            Console.WriteLine("\n\n\n\n\n\nDONE!");
            dungeon.Print();
        }
    }
}