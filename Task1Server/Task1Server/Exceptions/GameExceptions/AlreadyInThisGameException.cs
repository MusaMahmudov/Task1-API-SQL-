namespace Task1Server.Exceptions.GameExceptions
{
    public class AlreadyInThisGameException : Exception
    {
        public AlreadyInThisGameException(string message) : base(message) 
        {
        
        }
    }
}
