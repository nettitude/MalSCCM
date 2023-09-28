using System;
using System.Collections.Generic;

namespace MalSCCM.Commands;

public class Group : ICommand
{
    public string CommandName => "group";
    
    public static string GroupName = "";
    public static string GroupType = "";
    public static string SystemCollectionID = "";
    public static string UserCollectionID = "";
    public static string TargetCollectionID = "";
    public static string UserName = "";
    public static string DeviceName = "";
    public static string ResourceID = "";

    public void Execute(Dictionary<string, string> arguments)
    {
        if (arguments.TryGetValue("/server", out var argument))
        {
            Inspect.ServerName = argument;
        }

        Console.WriteLine("[*] Action: Manipulating SCCM Groups");

        if (arguments.TryGetValue("/groupname", out var argument1))
        {
            GroupName = argument1;
        }

        if (arguments.TryGetValue("/grouptype", out var argument2))
        {
            GroupType = argument2;
        }

        if (arguments.TryGetValue("/user", out var argument3))
        {
            UserName = argument3;
        }

        if (arguments.TryGetValue("/host", out var argument4))
        {
            DeviceName = argument4;
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

        if (arguments.ContainsKey("/create"))
        {
            Console.WriteLine("[*] Action: Creating SCCM Group");
            Console.WriteLine("\r\n[*] Action: Getting Collection IDs");
            Groups.FbGetSCCMCollectionID();
            Console.WriteLine("\r\n[*] Action: Creating Collection");
            Groups.FbNewSCCMCollection();
        }

        if (arguments.ContainsKey("/delete"))
        {
            Console.WriteLine("[*] Action: Deleting SCCM Group");
            Console.WriteLine("\r\n[*] Action: Getting Collection IDs");
            Groups.FbGetSCCMCollectionID();
            Console.WriteLine("\r\n[*] Action: Removing Collection");
            Groups.FbRemoveSCCMCollection();
        }

        if (arguments.ContainsKey("/adduser"))
        {
            Console.WriteLine("[*] Action: Adding User to an SCCM Group");
            Console.WriteLine("\r\n[*] Action: Getting Collection IDs");
            Groups.FbGetSCCMCollectionID();
            Console.WriteLine("\r\n[*] Action: Adding User");
            Groups.FbAddUserToSCCMCollection();
        }

        if (arguments.ContainsKey("/addhost"))
        {
            Console.WriteLine("[*] Action: Adding System to an SCCM Group");
            Console.WriteLine("\r\n[*] Action: Getting Collection IDs");
            Groups.FbGetSCCMCollectionID();
            Console.WriteLine("\r\n[*] Action: Adding Device");
            Groups.FbAddDeviceToSCCMCollection();
        }

        Console.WriteLine("\r\n[*] Group complete\r\n");
    }
}