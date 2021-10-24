using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Model
{
    public class Logs
    {
        private readonly Stack<Log> logs = new();

        public Log Current { get => logs.Count == 0 ? Push() : logs.Peek(); }

        public Log Push() { logs.Push(new()); return Current; }
        public Log Pop() => logs.Count == 0 ? new() : logs.Pop();
    }
}
