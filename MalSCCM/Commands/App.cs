using System;
using System.Collections.Generic;

namespace MalSCCM.Commands;

public class App : ICommand
{
    public string CommandName => "app";
    
    public static string AppName = "";
    public static string UNCPath = "";
    public static string AssignmentName = "";

    public void Execute(Dictionary<string, string> arguments)
    {
        if (arguments.TryGetValue("/server", out var argument))
        {
            Inspect.ServerName = argument;
        }

        Console.WriteLine("[*] Action: Manipulating SCCM Applications");

        if (arguments.TryGetValue("/groupname", out var argument1))
        {
            Group.GroupName = argument1;
        }

        if (arguments.TryGetValue("/name", out var argument2))
        {
            AppName = argument2;
        }

        if (arguments.TryGetValue("/uncpath", out var argument3))
        {
            UNCPath = argument3;
        }

        if (arguments.TryGetValue("/assignmentname", out var argument4))
        {
            AssignmentName = argument4;
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
            Console.WriteLine("[*] Action: Creating SCCM Application");
            Application.FbCreateSCCMApplication();
        }

        if (arguments.ContainsKey("/delete"))
        {
            Console.WriteLine("[*] Action: Deleting SCCM Application");
            Application.FbRemoveSCCMApplication();
        }

        if (arguments.ContainsKey("/deploy"))
        {
            Console.WriteLine("[*] Action: Gathering group ID");
            Groups.FbGetSCCMCollectionID();
            Console.WriteLine("[*] Action: Deploying SCCM Application");
            Application.FbDeploySCCMApplication();
        }

        if (arguments.ContainsKey("/deletedeploy"))
        {
            Console.WriteLine("[*] Action: Removing SCCM Application Deployment");
            Application.FbRemoveSCCMApplicationDeployment();
        }

        if (arguments.ContainsKey("/cleanup"))
        {
            Console.WriteLine("[*] Action: Removing SCCM Application Deployment");
            Application.FbRemoveSCCMApplicationDeployment();
            Console.WriteLine("[*] Action: Deleting SCCM Application");
            Application.FbRemoveSCCMApplication();
        }

        Console.WriteLine("\r\n[*] App complete\r\n");
    }
}