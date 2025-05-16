namespace LinkBotLogic.Exceptions
{
    [Serializable]
    public class MessagePageException : Exception
    {
        public MessagePageException()
        {
        }

        public MessagePageException(string? message) : base(message)
        {
        }

        public MessagePageException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}