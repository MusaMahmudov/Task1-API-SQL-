namespace Task1Server.Exceptions.GameExceptions
{
    public class GameIsFullException : Exception
    {
        public GameIsFullException(string message) : base(message) { }
    }
}
