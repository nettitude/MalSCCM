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
        if (arguments.ContainsKey("/server"))
        {
            Inspect.ServerName = arguments["/server"];
        }

        Console.WriteLine("[*] Action: Manipulating SCCM Applications");

        if (arguments.ContainsKey("/groupname"))
        {
            Group.GroupName = arguments["/groupname"];
        }

        if (arguments.ContainsKey("/name"))
        {
            AppName = arguments["/name"];
        }

        if (arguments.ContainsKey("/uncpath"))
        {
            UNCPath = arguments["/uncpath"];
        }

        if (arguments.ContainsKey("/assignmentname"))
        {
            AssignmentName = arguments["/assignmentname"];
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