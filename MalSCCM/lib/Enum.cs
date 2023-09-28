using System;
using System.Linq;
using System.Management;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.Win32;

using MalSCCM.Commands;

public static class Enum
{
    public static bool FbGetSiteScope()
    {
        try
        {
            var osQuery = new SelectQuery("SMS_Authority");
            var mgmtScope = new ManagementScope($@"\\{Inspect.ServerName}\root\ccm");
            mgmtScope.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(mgmtScope, osQuery);

            foreach (var result in mgmtSrchr.Get())
            {
                var siteCode = result.GetPropertyValue("Name").ToString();
                var managementServer = result.GetPropertyValue("CurrentManagementPoint").ToString();
                
                if (!string.IsNullOrEmpty(siteCode))
                {
                    Console.WriteLine("SiteCode: " + siteCode.Remove(0, 4));
                    Console.WriteLine("ManagementPoint: " + managementServer);

                    Inspect.SiteCode = siteCode.Remove(0, 4);

                    return true;
                }
                
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSiteScope.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSiteScope2()
    {
        try
        {
            var osQuery = new SelectQuery("SMS_ProviderLocation");
            var mgmtScope = new ManagementScope($@"\\{Inspect.ServerName}\root\sms");
            mgmtScope.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(mgmtScope, osQuery);

            foreach (var result in mgmtSrchr.Get())
            {
                var siteCode = result.GetPropertyValue("SiteCode").ToString();
                var localsite = result.GetPropertyValue("ProviderForLocalSite").ToString();

                if (!string.IsNullOrEmpty(siteCode))
                {
                    Console.WriteLine("SiteCode: " + siteCode);

                    Inspect.SiteCode = siteCode;

                    return true;
                }
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSiteScope2.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSiteScope3()
    {
        try
        {
            const string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SMS\Mobile Client";
            var assignedsitecode = (string)Registry.GetValue(keyName, "AssignedSiteCode", "No assigned site found, is this machine managed by SCCM?");

            Console.WriteLine("SiteCode: " + assignedsitecode);
            Inspect.SiteCode = assignedsitecode;

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSiteScope3.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMComputer()
    {
        try
        {
            var Query = new SelectQuery("SMS_R_System");
            var SCCMNamespace = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var CompName = result.GetPropertyValue("Name").ToString();

                if (!string.IsNullOrEmpty(CompName))
                {
                    Console.WriteLine("Computer: " + CompName);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMComputer.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMADForest()
    {
        try
        {
            var Query = new SelectQuery("SMS_ADForest");
            var SCCMNamespace = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var Forest = result.GetPropertyValue("Description").ToString();

                if (!string.IsNullOrEmpty(Forest))
                {
                    Console.WriteLine("Forest: " + Forest);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMADForest.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMApplication()
    {
        try
        {
            var Query = new SelectQuery("SMS_Application");
            var SCCMNamespace = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var DisplayName = result.GetPropertyValue("LocalizedDisplayName").ToString();

                if (!string.IsNullOrEmpty(DisplayName))
                {
                    Console.WriteLine("Application Name: " + DisplayName);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMApplication.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMPackage()
    {
        try
        {
            var Query = new SelectQuery("SMS_Package");
            var SCCMNamespace = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var Source = result.GetPropertyValue("PkgSourcePath").ToString();

                if (!string.IsNullOrEmpty(Source))
                {
                    Console.WriteLine("Package Source: " + Source);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMPackage.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMCollection()
    {
        try
        {
            var Query = new SelectQuery("SMS_Collection");
            var SCCMNamespace = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var Name = result.GetPropertyValue("Name").ToString();
                var Count = result.GetPropertyValue("MemberCount").ToString();

                if (!string.IsNullOrEmpty(Name))
                {
                    Console.WriteLine("Group: " + Name);
                }
                if (!string.IsNullOrEmpty(Count))
                {
                    Console.WriteLine("Member Count: " + Count);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMCollection.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }

    public static bool FbGetSCCMPrimaryUser()
    {
        try
        {
            var Query = new SelectQuery("SMS_UserMachineRelationship");
            var SCCMNamespace = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var ResourceName = result.GetPropertyValue("ResourceName").ToString();
                var UniqueUserName = result.GetPropertyValue("UniqueUserName").ToString();

                if (!string.IsNullOrEmpty(ResourceName))
                {
                    Console.WriteLine("Computer: " + ResourceName);
                }
                if (!string.IsNullOrEmpty(UniqueUserName))
                {
                    Console.WriteLine("User: " + UniqueUserName);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMPrimaryUser.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMDeployments()
    {
        try
        {
            var Query = new SelectQuery("SMS_ApplicationAssignment");
            var SCCMNamespace = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            SCCMNamespace.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(SCCMNamespace, Query);

            foreach (var result in mgmtSrchr.Get())
            {
                var AppName = result.GetPropertyValue("ApplicationName").ToString();
                var AssignmentName = result.GetPropertyValue("AssignmentName").ToString();

                if (!string.IsNullOrEmpty(AppName))
                {
                    Console.WriteLine("ApplicationName: " + AppName);
                }
                if (!string.IsNullOrEmpty(AssignmentName))
                {
                    Console.WriteLine("AssignmentName: " + AssignmentName);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMDeployments.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMPrimaryServerRegKey()
    {
        try
        {
            const string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SMS\DP";
            var mgmtServer = (string)Registry.GetValue(keyName, "ManagementPoints", "Management key not found, are you an SCCM client?");
            var siteServer = (string)Registry.GetValue(keyName, "SiteServer", "Key not found, are you on a management server?");

            const string keyNameID = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SMS\Identification";
            var siteServerID = (string)Registry.GetValue(keyNameID, "Site Server", "Key not found, are you on a management server?");

            Console.WriteLine("Management Server: {0}", mgmtServer);
            Console.WriteLine("Primary Server: {0}", siteServer);
            Console.WriteLine("Primary Server (alternate reg key): {0}", siteServerID);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMPrimaryServerRegKey.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    
    public static bool FbGetSCCMAdmins()
    {
        try
        {
            var query = new SelectQuery("SMS_Admin");
            var scope = new ManagementScope($@"\\{Inspect.ServerName}\root\sms\site_{Inspect.SiteCode}");
            scope.Connect();

            var searcher = new ManagementObjectSearcher(scope, query);

            foreach (var result in searcher.Get())
            {
                var logonName = result.GetPropertyValue("LogonName").ToString();
                var adminSid = result.GetPropertyValue("AdminSid").ToString();
                var roleNames = result.GetPropertyValue("RoleNames") as string[] ?? Array.Empty<string>();
                var categoryNames = result.GetPropertyValue("CategoryNames") as string[] ?? Array.Empty<string>();
                var collectionNames = result.GetPropertyValue("CollectionNames") as string[] ?? Array.Empty<string>();

                Console.WriteLine("UserName: {0}", logonName);
                Console.WriteLine("SID: {0}", adminSid);
                Console.WriteLine("Roles: {0}", string.Join(", ", roleNames));
                Console.WriteLine("Security Scopes: {0}", string.Join(", ", categoryNames));
                Console.WriteLine("Collections: {0}", string.Join(", ", collectionNames));
                Console.WriteLine();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetSCCMComputer.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
}
