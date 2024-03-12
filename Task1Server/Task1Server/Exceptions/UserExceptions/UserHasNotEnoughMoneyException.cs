namespace Task1Server.Exceptions.UserExceptions
{
    public class UserHasNotEnoughMoneyException : Exception
    {
        public UserHasNotEnoughMoneyException(string message) : base(message) { }
    }
}
