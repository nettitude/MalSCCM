using System;
using System.Collections.Generic;
using System.Linq;

using MalSCCM.Commands;

namespace MalSCCM.Args;

public class CommandCollection
{
    private readonly List<ICommand> _availableCommands = new();

    // How To Add A New Command:
    //  - Create your command class in the Commands Folder
    //    - That class must implement the ICommand interface
    //    - Give the command a name
    //    - Put the code that does the work into the Execute() method

    public CommandCollection()
    {
        // instantiate each command dynamically
        
        var self = typeof(CommandCollection).Assembly;
        
        // loop through each type
        foreach (var type in self.GetTypes())
        {
            // ignore if they don't implement ICommand or if it's the interface itself
            if (!typeof(ICommand).IsAssignableFrom(type) || type.Name.Equals("ICommand"))
                continue;
            
            // instantiate a new instance
            var command = (ICommand)Activator.CreateInstance(type);
            _availableCommands.Add(command);
        }
    }

    public bool ExecuteCommand(string commandName, Dictionary<string, string> arguments)
    {
        // find the correct command, case-insensitive 
        var command = _availableCommands.FirstOrDefault(c =>
            c.CommandName.Equals(commandName, StringComparison.OrdinalIgnoreCase));

        // return false if command is null (i.e. not found)
        if (command is null)
            return false;
        
        // otherwise execute and return true
        command.Execute(arguments);
        return true;
    }
}