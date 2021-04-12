namespace Necromancy.Cli.Argument
{
    public class SwitchProperty<T> : ISwitchProperty
    {
        public delegate void AssignHandler(T result);

        public delegate bool TryParseHandler(string value, out T result);

        public static TryParseHandler noOp = (string value, out T result) =>
        {
            result = default;
            return true;
        };

        private readonly AssignHandler _assigner;

        private readonly TryParseHandler _parser;

        public SwitchProperty(string key, string valueDescription, string description, TryParseHandler parser,
            AssignHandler assigner)
        {
            this.key = key;
            this.valueDescription = valueDescription;
            this.description = description;
            _parser = parser;
            _assigner = assigner;
        }

        public string key { get; }
        public string description { get; }
        public string valueDescription { get; }

        public bool Assign(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            if (!_parser(value, out T result)) return false;

            _assigner(result);
            return true;
        }
    }
}
