using System.Collections.Generic;

namespace Necromancy.Cli.Argument
{
    public interface ISwitchConsumer
    {
        List<ISwitchProperty> switches { get; }
    }
}
