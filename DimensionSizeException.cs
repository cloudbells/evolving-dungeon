namespace Evolving_Dungeon
{
    /// <summary>
    /// Should be thrown when the given dungeon dimension is smaller than allowed.
    /// </summary>
    internal class DimensionSizeException : Exception
    {

        public DimensionSizeException()
        {
        }

        public DimensionSizeException(string message) : base(message)
        {
        }

        public DimensionSizeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
