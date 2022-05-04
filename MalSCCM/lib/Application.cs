using System;
using System.Management;
using System.Text;
using MalSCCM.Commands;

public class Application
{
    public static bool FbCreateSCCMApplication()
    {
        try
        {
            ManagementClass IDClass = new ManagementClass($"\\\\{Inspect.ServerName}\\root\\sms\\site_{Inspect.SiteCode}:SMS_Identification");
            ManagementClass AppClass = new ManagementClass($"\\\\{Inspect.ServerName}\\root\\sms\\site_{Inspect.SiteCode}:SMS_Application");

            object[] methodArgs = {null};

            object result = IDClass.InvokeMethod("GetSiteID", methodArgs);
            string scopeid = (string)methodArgs[0];
            var trimscopeid = "ScopeId_" + scopeid.Trim(new char[] { '{', '}' });

            Console.WriteLine("ScopeID: " + trimscopeid);

            var NewAppID = "Application_" + Guid.NewGuid();
            Console.WriteLine("NewAppID: " + NewAppID);
            var NewDeployID = "DeploymentType_" + Guid.NewGuid();
            Console.WriteLine("NewDeployID: " + NewDeployID);
            var NewFileID = "File_" + Guid.NewGuid();
            Console.WriteLine("NewFileID: " + NewFileID);

            StringBuilder xml = new StringBuilder();

            xml.AppendLine(@"<?xml version=""1.0"" encoding=""utf-16""?><AppMgmtDigest xmlns=""http://schemas.microsoft.com/SystemCenterConfigurationManager/2009/AppMgmtDigest"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><Application AuthoringScopeId=""" + trimscopeid + @""" LogicalName=""" + NewAppID + @""" Version=""2""><DisplayInfo DefaultLanguage=""en-US""><Info Language=""en-US""><Title>" + App.AppName + @"</Title><Publisher/><Version/></Info></DisplayInfo><DeploymentTypes><DeploymentType AuthoringScopeId=""" + trimscopeid + @""" LogicalName=""" + NewDeployID + @""" Version=""2""/></DeploymentTypes><Title ResourceId=""Res_684364143"">" + App.AppName + @"</Title><Description ResourceId=""Res_1018411239""/><Publisher ResourceId=""Res_1340020890""/><SoftwareVersion ResourceId=""Res_597041892""/><CustomId ResourceId=""Res_872061892""/></Application><DeploymentType AuthoringScopeId=""" + trimscopeid + @""" LogicalName=""" + NewDeployID + @""" Version=""2""><Title ResourceId=""Res_1244298486"">" + App.AppName + @"</Title><Description ResourceId=""Res_405397997""/><DeploymentTechnology>GLOBAL/ScriptDeploymentTechnology</DeploymentTechnology><Technology>Script</Technology><Hosting>Native</Hosting><Installer Technology=""Script""><ExecutionContext>System</ExecutionContext><DetectAction><Provider>Local</Provider><Args><Arg Name=""ExecutionContext"" Type=""String"">System</Arg><Arg Name=""MethodBody"" Type=""String"">&lt;?xml version=""1.0"" encoding=""utf-16""?&gt;");
            xml.AppendLine(@"&lt;EnhancedDetectionMethod xmlns=""http://schemas.microsoft.com/SystemCenterConfigurationManager/2009/AppMgmtDigest""&gt;");
            xml.AppendLine("\t" + @"&lt;Settings xmlns=""http://schemas.microsoft.com/SystemCenterConfigurationManager/2009/AppMgmtDigest""&gt;");
            xml.AppendLine("\t\t" + @"&lt;File Is64Bit=""false"" LogicalName=""" + NewFileID + @""" xmlns=""http://schemas.microsoft.com/SystemsCenterConfigurationManager/2009/07/10/DesiredConfiguration""&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;Annotation xmlns=""http://schemas.microsoft.com/SystemsCenterConfigurationManager/2009/06/14/Rules""&gt;");
            xml.AppendLine("\t\t\t\t" + @"&lt;DisplayName Text="""" /&gt;");
            xml.AppendLine("\t\t\t\t" + @"&lt;Description Text="""" /&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;/Annotation&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;Path&gt;C:\&lt;/Path&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;Filter&gt;asdf&lt;/Filter&gt;");
            xml.AppendLine("\t\t" + @"&lt;/File&gt;");
            xml.AppendLine("\t" + @"&lt;/Settings&gt;");
            xml.AppendLine("\t" + @"&lt;Rule id=""" + trimscopeid + "/" + NewDeployID + @""" Severity=""Informational"" NonCompliantWhenSettingIsNotFound=""false"" xmlns=""http://schemas.microsoft.com/SystemsCenterConfigurationManager/2009/06/14/Rules""&gt;");
            xml.AppendLine("\t\t" + @"&lt;Annotation&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;DisplayName Text="""" /&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;Description Text="""" /&gt;");
            xml.AppendLine("\t\t" + @"&lt;/Annotation&gt;");
            xml.AppendLine("\t\t" + @"&lt;Expression&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;Operator&gt;NotEquals&lt;/Operator&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;Operands&gt;");
            xml.AppendLine("\t\t\t\t" + @"&lt;SettingReference AuthoringScopeId=""" + trimscopeid + @""" LogicalName=""" + NewAppID + @""" Version=""2"" DataType=""Int64"" SettingLogicalName=""" + NewFileID + @""" SettingSourceType=""File"" Method=""Count"" Changeable=""false"" /&gt;");
            xml.AppendLine("\t\t\t\t" + @"&lt;ConstantValue Value=""0"" DataType=""Int64"" /&gt;");
            xml.AppendLine("\t\t\t" + @"&lt;/Operands&gt;");
            xml.AppendLine("\t\t" + @"&lt;/Expression&gt;");
            xml.AppendLine("\t" + @"&lt;/Rule&gt;");
            xml.AppendLine(@"&lt;/EnhancedDetectionMethod&gt;</Arg></Args></DetectAction><InstallAction><Provider>Script</Provider><Args><Arg Name=""InstallCommandLine"" Type=""String"">" + App.UNCPath + @"</Arg><Arg Name=""WorkingDirectory"" Type=""String"">C:\Windows\System32</Arg><Arg Name=""ExecutionContext"" Type=""String"">System</Arg><Arg Name=""RequiresLogOn"" Type=""String""/><Arg Name=""RequiresElevatedRights"" Type=""Boolean"">false</Arg><Arg Name=""RequiresUserInteraction"" Type=""Boolean"">false</Arg><Arg Name=""RequiresReboot"" Type=""Boolean"">false</Arg><Arg Name=""UserInteractionMode"" Type=""String"">Hidden</Arg><Arg Name=""PostInstallBehavior"" Type=""String"">BasedOnExitCode</Arg><Arg Name=""ExecuteTime"" Type=""Int32"">0</Arg><Arg Name=""MaxExecuteTime"" Type=""Int32"">120</Arg><Arg Name=""RunAs32Bit"" Type=""Boolean"">false</Arg><Arg Name=""SuccessExitCodes"" Type=""Int32[]""><Item>0</Item><Item>1707</Item></Arg><Arg Name=""RebootExitCodes"" Type=""Int32[]""><Item>3010</Item></Arg><Arg Name=""HardRebootExitCodes"" Type=""Int32[]""><Item>1641</Item></Arg><Arg Name=""FastRetryExitCodes"" Type=""Int32[]""><Item>1618</Item></Arg></Args></InstallAction><CustomData><DetectionMethod>Enhanced</DetectionMethod><EnhancedDetectionMethod><Settings xmlns=""http://schemas.microsoft.com/SystemCenterConfigurationManager/2009/AppMgmtDigest""><File xmlns=""http://schemas.microsoft.com/SystemsCenterConfigurationManager/2009/07/10/DesiredConfiguration"" Is64Bit=""false"" LogicalName=""" + NewFileID + @"""><Annotation xmlns=""http://schemas.microsoft.com/SystemsCenterConfigurationManager/2009/06/14/Rules""><DisplayName Text=""""/><Description Text=""""/></Annotation><Path>C:\</Path><Filter>asdf</Filter></File></Settings><Rule xmlns=""http://schemas.microsoft.com/SystemsCenterConfigurationManager/2009/06/14/Rules"" id=""" + trimscopeid + "/" + NewDeployID + @""" Severity=""Informational"" NonCompliantWhenSettingIsNotFound=""false""><Annotation><DisplayName Text=""""/><Description Text=""""/></Annotation><Expression><Operator>NotEquals</Operator><Operands><SettingReference AuthoringScopeId=""" + trimscopeid + @""" LogicalName=""" + NewAppID + @""" Version=""2"" DataType=""Int64"" SettingLogicalName=""" + NewFileID + @""" SettingSourceType=""File"" Method=""Count"" Changeable=""false""/><ConstantValue Value=""0"" DataType=""Int64""/></Operands></Expression></Rule></EnhancedDetectionMethod><InstallCommandLine>" + App.UNCPath + @"</InstallCommandLine><InstallFolder>C:\Windows\System32</InstallFolder><ExitCodes><ExitCode Code=""0"" Class=""Success""/><ExitCode Code=""1707"" Class=""Success""/><ExitCode Code=""3010"" Class=""SoftReboot""/><ExitCode Code=""1641"" Class=""HardReboot""/><ExitCode Code=""1618"" Class=""FastRetry""/></ExitCodes><UserInteractionMode>Hidden</UserInteractionMode><AllowUninstall>true</AllowUninstall></CustomData></Installer></DeploymentType></AppMgmtDigest>");

            Console.WriteLine("Creating Instance");

            ManagementObject newInstance = AppClass.CreateInstance();
            
            newInstance["SDMPackageXML"] = xml.ToString();
            newInstance["IsHidden"] = true;

            newInstance.Put();

            Console.WriteLine("App Created");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbCreateSCCMApplication.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbRemoveSCCMApplication()
    {
        try
        {
            var Query = new SelectQuery($"Select * FROM SMS_Application WHERE LocalizedDisplayName = '{App.AppName}'");
            var mgmtScope = new ManagementScope($"\\\\{Inspect.ServerName}\\root\\sms\\site_{Inspect.SiteCode}");
            mgmtScope.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(mgmtScope, Query);
            ManagementObjectCollection objColl = mgmtSrchr.Get();

            foreach (ManagementObject obj in objColl)
            {
                object[] methodArgs = { "True" };
                obj.InvokeMethod("SetIsExpired", methodArgs);
                Console.WriteLine("App retired");

                obj.Delete();
                Console.WriteLine("App deleted");
            }
            Console.WriteLine("App has been deleted successfully");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbRemoveSCCMApplication.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbDeploySCCMApplication()
    {
        try
        {
            ManagementClass AppAssignementClass = new ManagementClass($"\\\\{Inspect.ServerName}\\root\\sms\\site_{Inspect.SiteCode}:SMS_ApplicationAssignment");
            var TargetCollectionID = Group.TargetCollectionID;

            var Query = new SelectQuery($"Select * FROM SMS_Application WHERE LocalizedDisplayName = '{App.AppName}'");
            var mgmtScope = new ManagementScope($"\\\\{Inspect.ServerName}\\root\\sms\\site_{Inspect.SiteCode}");
            mgmtScope.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(mgmtScope, Query);
            var CI_ID = "";
            int CI_IDint = 0;
            var CI_UniqueID = "";

            foreach (var result in mgmtSrchr.Get())
            {
                CI_ID = result.GetPropertyValue("CI_ID").ToString();
                CI_IDint = int.Parse(CI_ID);
                CI_UniqueID = result.GetPropertyValue("CI_UniqueID").ToString();
            }

            var Date = DateTime.Now.ToString("yyyyMMddHHmmss") + ".000000+***";

            ManagementObject newInstance = AppAssignementClass.CreateInstance();

            newInstance["ApplicationName"] = App.AppName;
            newInstance["AssignmentName"] = App.AssignmentName;
            newInstance["AssignedCIs"] = new int[] { CI_IDint };
            newInstance["AssignmentAction"] = 2;
            newInstance["DesiredConfigType"] = 1;
            newInstance["LogComplianceToWinEvent"] = false;
            newInstance["CollectionName"] = Group.GroupName;
            newInstance["CreationTime"] = Date;
            newInstance["LocaleID"] = 1033;
            newInstance["SourceSite"] = Inspect.SiteCode;
            newInstance["StartTime"] = Date;
            newInstance["DisableMOMAlerts"] = true;
            newInstance["SuppressReboot"] = false;
            newInstance["NotifyUser"] = false;
            newInstance["TargetCollectionID"] = TargetCollectionID;
            newInstance["EnforcementDeadline"] = Date;
            newInstance["OfferTypeID"] = 0;
            newInstance["OfferFlags"] = 0;
            newInstance["Priority"] = 2;
            newInstance["UserUIExperience"] = false;
            newInstance["WoLEnabled"] = false;
            newInstance["RebootOutsideOfServiceWindows"] = false;
            newInstance["OverrideServiceWindows"] = false;
            newInstance["UseGMTTimes"] = true;
            newInstance.Put();

            Console.WriteLine("App Deployed");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbDeploySCCMApplication.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }

    public static bool FbRemoveSCCMApplicationDeployment()
    {
        try
        {
            var Query = new SelectQuery("Select * FROM SMS_ApplicationAssignment WHERE ApplicationName = '" + App.AppName + "'");
            var mgmtScope = new ManagementScope($"\\\\{Inspect.ServerName}\\root\\sms\\site_{Inspect.SiteCode}");
            mgmtScope.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(mgmtScope, Query);
            ManagementObjectCollection objColl = mgmtSrchr.Get();

            foreach (ManagementObject obj in objColl)
            {
                obj.Delete();
                Console.WriteLine("object deleted");
            }

            Console.WriteLine("App deployment has been deleted successfully");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbRemoveSCCMApplicationDeployment.");
            var stdErr = Console.Error;
            stdErr.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }

}
