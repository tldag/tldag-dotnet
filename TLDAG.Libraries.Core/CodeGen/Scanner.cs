namespace TLDAG.Libraries.Core.CodeGen
{
    public class Token
    {
        public SourcePosition Position { get; }
        public string Name { get; }
        public string Value { get; }

        public Token(SourcePosition position, string name, string value)
        {
            Position = position;
            Name = name;
            Value = value;
        }
    }

    public class Scanner
    {
    }
}
