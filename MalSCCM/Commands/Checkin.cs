using System;
using System.Collections.Generic;

namespace MalSCCM.Commands
{
    public class Checkin : ICommand
    {

        public static string CommandName => "checkin";


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
            Console.WriteLine("\r\n[*] App complete\r\n");
        }
    }
}