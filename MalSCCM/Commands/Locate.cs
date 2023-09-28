using System;
using System.Collections.Generic;

namespace MalSCCM.Commands;

public class Locate : ICommand
{

    public static string CommandName => "locate";
    public static string SiteCode = "";
    public static string ServerName = "localhost";

    public void Execute(Dictionary<string, string> arguments)
    {
        if (arguments.ContainsKey("/server"))
        {
            ServerName = arguments["/server"];
        }

        Console.WriteLine("[*] Action: Locating SCCM Management Servers");

        if (!Enum.FbGetSiteScope())
        {
            Console.WriteLine("Getting sitecode from CCM namespace failed, trying SMS instead");
            if (!Enum.FbGetSiteScope2())
            {
                Console.WriteLine("Getting sitecode from WMI failed, attempting client registry keys");
                Enum.FbGetSiteScope3();
            }
        }

        Console.WriteLine("\r\n[!] Note - Managment Server may not be the Primary Server which is needed for exploitation.");
        Console.WriteLine("[!] Note - You can try use 'inspect /server:<managementserver>' to see if the management server is exploitable.");
        Console.WriteLine("[!] Note - If you are on a management server, the registry checks below should return the primary server");

        Console.WriteLine("\r\n[*] Action: Locating SCCM Servers in Registry");

        Enum.FbGetSCCMPrimaryServerRegKey();

        Console.WriteLine("\r\n[!] Note - If looking for reg keys failed, make sure you are on a management server!");
        Console.WriteLine("[!] Note - Alternate ways of finding the primary server could be shares on the network (SMS_<sitecode>) will be the name of a share on the primary server.");

        Console.WriteLine("\r\n[*] Locate complete\r\n");
    }
}