namespace Task1Server.Exceptions.UserExceptions
{
    public class SignInException : Exception
    {
        public SignInException(string message) :base(message) { }
    }
}
