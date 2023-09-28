using System;
using System.Management;

using MalSCCM.Commands;

public static class Check
{
    public static bool FbSCCMDeviceCheckin()
    {
        try
        {
            var Class = new ManagementClass($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}:SMS_ClientOperation");
            var newInstance = Class.GetMethodParameters("InitiateClientOperation");

            newInstance["Type"] = 8;
            newInstance["TargetCollectionID"] = Group.TargetCollectionID;

            var result = Class.InvokeMethod("InitiateClientOperation",newInstance,null);

            Console.WriteLine("ReturnValue: " + result.GetPropertyValue("ReturnValue"));
            Console.WriteLine("OperationID: " + result.GetPropertyValue("OperationID"));

            Console.WriteLine("Checkin succeeded.");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbSCCMDeviceCheckin.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
}
