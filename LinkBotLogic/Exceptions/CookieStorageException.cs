namespace LinkBotLogic.Exceptions
{
    [Serializable]
    public class CookieStorageException : Exception
    {
        public CookieStorageException()
        {
        }

        public CookieStorageException(string? message) : base(message)
        {
        }

        public CookieStorageException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}