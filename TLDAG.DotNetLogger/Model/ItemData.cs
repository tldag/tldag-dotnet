namespace TLDAG.DotNetLogger.Model
{
    public class ItemData
    {
        public string Type { get; }
        public string Spec { get; }

        public ItemData(string type, string spec)
        {
            Type = type;
            Spec = spec;
        }
    }
}
