namespace Necromancy.Cli.Argument
{
    public interface ISwitchProperty
    {
        string key { get; }
        string description { get; }
        string valueDescription { get; }
        bool Assign(string value);
    }
}
