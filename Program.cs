namespace Evolving_Dungeon
{
    /// <summary>
    /// Main entry point.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            DungeonGenerator generator = new DungeonGenerator();
            Dungeon dungeon = generator.Generate(20, 10);
            Console.WriteLine("\n\n\n\n\n\nDONE!");
            dungeon.Print();
        }
    }
}