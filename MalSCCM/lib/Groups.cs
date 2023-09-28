using System;
using System.Management;

using MalSCCM.Commands;

public static class Groups
{
    public static bool FbGetSCCMCollectionID()
    {
        try
        {
            var Query = new SelectQuery("SMS_Collection");
            var SCCMNamespace = new ManagementScope($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode);
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var Name = result.GetPropertyValue("Name").ToString();
             
                if (Name == "All Systems")
                {
                    var ID = result.GetPropertyValue("CollectionID").ToString();
                    Console.WriteLine("SystemCollectionID: " + ID);
                    Group.SystemCollectionID = ID;
                }
                if (Name == "All Users")
                {
                    var ID = result.GetPropertyValue("CollectionID").ToString();
                    Console.WriteLine("UserCollectionID: " + ID);
                    Group.UserCollectionID = ID;
                }

                if (Name == Group.GroupName)
                {
                    var ID = result.GetPropertyValue("CollectionID").ToString();
                    Console.WriteLine("TargetGroupID: " + ID);
                    Group.TargetCollectionID = ID;
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMCollectionID.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbNewSCCMCollection()
    {
        try
        {
            var Class = new ManagementClass($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode + ":SMS_Collection");
            var newInstance = Class.CreateInstance();

            newInstance["Name"] = Group.GroupName;
            newInstance["OwnedByThisSite"] = "True";

            if (Group.GroupType == "user")
            {
                Console.WriteLine("Setting up user group type");
                newInstance["LimitToCollectionID"] = Group.UserCollectionID;
                newInstance["CollectionType"] = 1;
            }

            if (Group.GroupType == "device")
            {
                Console.WriteLine("Setting up device group type");
                newInstance["LimitToCollectionID"] = Group.SystemCollectionID;
                newInstance["CollectionType"] = 2;
            }

            Console.WriteLine("Commiting instance");
            newInstance.Put(); //to commit the new instance.

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbNewSCCMCollection.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbRemoveSCCMCollection()
    {
        try
        {
            var objHostSetting = new ManagementObject();
            objHostSetting.Scope = new ManagementScope($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode);

            //define lookup query  
            var strQuery = @"SMS_Collection.CollectionID='" + Group.TargetCollectionID + "'";
            objHostSetting.Path = new ManagementPath(strQuery);

            //delete the Managementobject  
            objHostSetting.Delete();

            Console.WriteLine("Group has been deleted successfully");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbRemoveSCCMCollection.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetUserResourceID()
    {
        try
        {
            var Query = new SelectQuery("SMS_R_User");
            var SCCMNamespace = new ManagementScope($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode);
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var Name = result.GetPropertyValue("UniqueUserName").ToString();

                if (Name == Group.UserName)
                {
                    var ID = result.GetPropertyValue("ResourceID").ToString();
                    Console.WriteLine("Resource: " + ID);
                    Group.ResourceID = ID;
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMCollectionID.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbAddUserToSCCMCollection()
    {
        try
        {
            var collQuery = new ManagementClass($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode, "SMS_CollectionRuleQuery", null);
            var collQueryInstance = collQuery.CreateInstance();

            collQueryInstance["QueryExpression"] = "Select * from SMS_R_User Where UniqueUserName='" + Group.UserName + "'";
            collQueryInstance["RuleName"] = "Members of collection";

            var collInstance = new ManagementObject($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode + ":SMS_Collection.CollectionID='" + Group.TargetCollectionID + "'");
            var inParams = collInstance.GetMethodParameters("AddMembershipRule");

            Console.WriteLine("Commiting instance");

            inParams.SetPropertyValue("collectionRule", collQueryInstance);

            var outParams = collInstance.InvokeMethod("AddMembershipRule", inParams, null);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbAddUserToSCCMCollection.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    // To Do
    public static bool FbRemoveUserFromSCCMCollection()
    {
        try
        {

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbRemoveUserFromSCCMCollection.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbAddDeviceToSCCMCollection()
    {
        try
        {
            var collQuery = new ManagementClass($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode, "SMS_CollectionRuleQuery", null);
            var collQueryInstance = collQuery.CreateInstance();

            collQueryInstance["QueryExpression"] = "Select * from SMS_R_System Where Name='" + Group.DeviceName + "'";
            collQueryInstance["RuleName"] = "Members of collection";

            var collInstance = new ManagementObject($"\\\\{Inspect.ServerName}\\root\\sms\\site_" + Inspect.SiteCode + ":SMS_Collection.CollectionID='" + Group.TargetCollectionID + "'");
            var inParams = collInstance.GetMethodParameters("AddMembershipRule");

            Console.WriteLine("Commiting instance");

            inParams.SetPropertyValue("collectionRule", collQueryInstance);

            var outParams = collInstance.InvokeMethod("AddMembershipRule", inParams, null);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbAddDeviceToSCCMCollection.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    // To Do
    public static bool FbRemoveDeviceFromSCCMCollection()
    {
        try
        {

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbRemoveDeviceFromSCCMCollection.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
}