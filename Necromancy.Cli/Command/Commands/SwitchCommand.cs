using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Logging;
using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command.Commands
{
    public class SwitchCommand : ConsoleCommand
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(SwitchCommand));

        private readonly List<ISwitchConsumer> _parameterConsumers;

        public SwitchCommand(List<ISwitchConsumer> parameterConsumers)
        {
            _parameterConsumers = parameterConsumers;
        }

        public override string key => "switch";
        public override string description => $"Changes configuration switches{Environment.NewLine}{ShowSwitches()}";

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            foreach (string key in parameter.switchMap.Keys)
            {
                ISwitchProperty property = FindSwitch(key);
                if (property == null)
                {
                    _Logger.Error($"Switch '{key}' not found");
                    continue;
                }

                string value = parameter.switchMap[key];
                if (!property.Assign(value))
                    _Logger.Error($"Switch '{key}' failed, value: '{value}' is invalid");
                else
                    _Logger.Info($"Applied {key}={value}");
            }

            foreach (string booleanSwitch in parameter.switches)
            {
                ISwitchProperty property = FindSwitch(booleanSwitch);
                if (property == null)
                {
                    _Logger.Error($"Switch '{booleanSwitch}' not found");
                    continue;
                }

                if (!property.Assign(bool.TrueString))
                    _Logger.Error($"Switch '{booleanSwitch}' failed, value: '{bool.TrueString}' is invalid");
                else
                    _Logger.Info($"Applied {booleanSwitch}={bool.TrueString}");
            }

            return CommandResultType.Completed;
        }

        private ISwitchProperty FindSwitch(string key)
        {
            foreach (ISwitchConsumer consumer in _parameterConsumers)
            foreach (ISwitchProperty property in consumer.switches)
                if (property.key == key)
                    return property;

            return null;
        }

        private string ShowSwitches()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Available Switches:");
            sb.Append(Environment.NewLine);
            foreach (ISwitchConsumer consumer in _parameterConsumers)
            foreach (ISwitchProperty property in consumer.switches)
            {
                sb.Append(Environment.NewLine);
                sb.Append(property.key);
                sb.Append(Environment.NewLine);
                sb.Append("> Ex.: ");
                sb.Append(property.valueDescription);
                sb.Append(Environment.NewLine);
                sb.Append("> ");
                sb.Append(property.description);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
