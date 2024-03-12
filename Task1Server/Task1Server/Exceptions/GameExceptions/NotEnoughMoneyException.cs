namespace Task1Server.Exceptions.GameExceptions
{
    public class NotEnoughMoneyException : Exception
    {
        public NotEnoughMoneyException(string message)  : base(message) 
            { }
    }
}
