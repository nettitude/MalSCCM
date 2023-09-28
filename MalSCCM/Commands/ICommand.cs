using System.Collections.Generic;

namespace MalSCCM.Commands;

public interface ICommand
{
    string CommandName { get; }
    void Execute(Dictionary<string, string> arguments);
}