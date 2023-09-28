using System;
using System.Collections.Generic;

namespace MalSCCM.Commands;

public class Checkin : ICommand
{
    public string CommandName => "checkin";

    public void Execute(Dictionary<string, string> arguments)
    {
        if (arguments.TryGetValue("/server", out var argument))
        {
            Inspect.ServerName = argument;
        }

        Console.WriteLine("[*] Action: Causing SCCM poll");

        if (arguments.TryGetValue("/groupname", out var argument1))
        {
            Group.GroupName = argument1;
        }

        if (!Enum.FbGetSiteScope())
        {
            Console.WriteLine("Getting sitecode from CCM namespace failed, trying SMS instead");
            if (!Enum.FbGetSiteScope2())
            {
                Console.WriteLine("Getting sitecode from WMI failed, attempting client registry keys");
                Enum.FbGetSiteScope3();
            }
        }

        if (arguments.ContainsKey("/groupname"))
        {
            Console.WriteLine("\r\n[*] Action: Getting Collection IDs");
            Groups.FbGetSCCMCollectionID();
            Console.WriteLine("[*] Action: Forcing Group To Checkin for Updates");
            Check.FbSCCMDeviceCheckin();
        }
        
        Console.WriteLine("\r\n[*] Checkin complete\r\n");
    }
}