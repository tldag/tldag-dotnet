namespace TLDAG.DotNetLogger.Model.Support
{
    public static class MessagesSupport
    {
        public static Messages? AddToMessages(Messages? messages, string? message)
        {
            if (message is null) return messages;
            if (string.IsNullOrWhiteSpace(message)) return messages;

            messages ??= new();
            messages.Lines.Add(message);

            return messages;
        }
    }
}
