using System.Collections.Generic;

namespace MalSCCM.Commands
{
    public interface ICommand
    {
        void Execute(Dictionary<string, string> arguments);
    }
}