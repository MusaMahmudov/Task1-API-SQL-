namespace Task1Server.Exceptions.GameExceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string message)  : base(message)
        { }
    }
}
