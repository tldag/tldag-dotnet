using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Model
{
    public interface IHasMessages
    {
        public Messages? Messages { get; set; }

        public void AddMessage(string? message);
    }
}
