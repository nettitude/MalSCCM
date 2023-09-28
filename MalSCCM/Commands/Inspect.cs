using System;
using System.Collections.Generic;

namespace MalSCCM.Commands;

public class Inspect : ICommand
{
    public string CommandName => "inspect";
    
    public static string SiteCode = "";
    public static string ServerName = "localhost";

    public void Execute(Dictionary<string, string> arguments)
    {
        if (arguments.TryGetValue("/server", out var argument))
        {
            ServerName = argument;
        }

        Console.WriteLine("[*] Action: Inspect SCCM Server");

        if (!Enum.FbGetSiteScope())
        {
            Console.WriteLine("Getting sitecode from CCM namespace failed, trying SMS instead");
            if (!Enum.FbGetSiteScope2())
            {
                Console.WriteLine("Getting sitecode from WMI failed, attempting client registry keys");
                Enum.FbGetSiteScope3();
            }
        }

        if (arguments.ContainsKey("/all"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Computers");
            Enum.FbGetSCCMComputer();
            Console.WriteLine("\r\n[*] Action: Get SCCM AD Forest");
            Enum.FbGetSCCMADForest();
            Console.WriteLine("\r\n[*] Action: Get SCCM Applications");
            Enum.FbGetSCCMApplication();
            Console.WriteLine("\r\n[*] Action: Get SCCM Packages");
            Enum.FbGetSCCMPackage();
            Console.WriteLine("\r\n[*] Action: Get SCCM Collections (Groups)");
            Enum.FbGetSCCMCollection();
            Console.WriteLine("\r\n[*] Action: Get SCCM Primary Users");
            Enum.FbGetSCCMPrimaryUser();
            Console.WriteLine("\r\n[*] Action: Get SCCM Deployments");
            Enum.FbGetSCCMDeployments();
            Console.WriteLine("\r\n[*] Action: Get SCCM Admins");
            Enum.FbGetSCCMAdmins();
        }

        if (arguments.ContainsKey("/computers"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Computers");
            Enum.FbGetSCCMComputer();
        }

        if (arguments.ContainsKey("/forest"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM AD Forest");
            Enum.FbGetSCCMADForest();
        }

        if (arguments.ContainsKey("/applications"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Applications");
            Enum.FbGetSCCMApplication();
        }

        if (arguments.ContainsKey("/packages"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Packages");
            Enum.FbGetSCCMPackage();
        }

        if (arguments.ContainsKey("/groups"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Collections (Groups)");
            Enum.FbGetSCCMCollection();
        }

        if (arguments.ContainsKey("/primaryusers"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Primary Users");
            Enum.FbGetSCCMPrimaryUser();
        }

        if (arguments.ContainsKey("/deployments"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Deployments");
            Enum.FbGetSCCMDeployments();
        }

        if (arguments.ContainsKey("/admins"))
        {
            Console.WriteLine("\r\n[*] Action: Get SCCM Admins");
            Enum.FbGetSCCMAdmins();
        }

        Console.WriteLine("\r\n[*] Inspect complete\r\n");
    }
}