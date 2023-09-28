using System;

namespace MalSCCM.Args;

public static class Info
{
    public static void ShowLogo()
    {
        const string logo = """
                             __  __       _ ____   ____ ____ __  __
                            |  \/  | __ _| / ___| / ___/ ___|  \/  |
                            | |\/| |/ _` | \___ \| |  | |   | |\/| |
                            | |  | | (_| | |___) | |__| |___| |  | |
                            |_|  |_|\__,_|_|____/ \____\____|_|  |_|
                                Phil Keeble @ Nettitude Red Team

                            """;
        
        Console.WriteLine(logo);
    }

    public static void ShowUsage()
    {
        const string usage = """
                             Commands listed below have optional parameters in <>.

                             Attempt to find the SCCM management and primary servers:
                                 MalSCCM.exe locate

                             Inspect the primary server to gather SCCM information:
                                 MalSCCM.exe inspect </server:PrimarySiteHostname> </all /computers /deployments /groups /applications /forest /packages /primaryusers /admins>

                             Create/Modify/Delete Groups to add targets in for deploying malicious apps. Groups can either be for devices or users:
                                 MalSCCM.exe group /create /groupname:example /grouptype:[user|device] </server:PrimarySiteHostname>
                                 MalSCCM.exe group /delete /groupname:example </server:PrimarySiteHostname>
                                 MalSCCM.exe group /addhost /groupname:example /host:examplehost </server:PrimarySiteHostname>
                                 MalSCCM.exe group /adduser /groupname:example /user:exampleuser </server:PrimarySiteHostname>

                             Create/Deploy/Delete malicious applications:
                                 MalSCCM.exe app /create /name:appname /uncpath:"\\unc\path" </server:PrimarySiteHostname>
                                 MalSCCM.exe app /delete /name:appname </server:PrimarySiteHostname>
                                 MalSCCM.exe app /deploy /name:appname /groupname:example /assignmentname:example2 </server:PrimarySiteHostname>
                                 MalSCCM.exe app /deletedeploy /name:appname </server:PrimarySiteHostname>
                                 MalSCCM.exe app /cleanup /name:appname </server:PrimarySiteHostname>

                             Force devices of a group to checkin within a couple minutes:
                                 MalSCCM.exe checkin /groupname:example </server:PrimarySiteHostname>

                             """;
        
        Console.WriteLine(usage);
    }
}