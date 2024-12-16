if (AntiCrackUtil.IsDebuggerPresent() || 
                !AntiCrackUtil.CheckFileIntegrity() || 
                AntiCrackUtil.IsRunningInVirtualMachine() || 
                AntiCrackUtil.CheckForCommonCrackTools())
            {
                MessageBox.Show("Se ha detectado actividad de cracking en esta aplicación.");
                return;
            }





=============================================================================


using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace AntiCrackByOnlyredzv1
{
    public static class AntiCrackUtil
    {
        public static bool IsDebuggerPresent()
        {
            return Debugger.IsAttached || NativeMethods.CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, out _);
        }

        public static bool CheckFileIntegrity()
        {
            string currentProcessPath = Process.GetCurrentProcess().MainModule.FileName;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(File.ReadAllBytes(currentProcessPath));
                string fileHash = BitConverter.ToString(hashBytes).Replace("-", "");
                return (fileHash == "YOUR_EXPECTED_HASH_HERE"); 
            }
        }

        public static bool IsRunningInVirtualMachine()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
            {
                foreach (var obj in searcher.Get())
                {
                    string manufacturer = obj["Manufacturer"].ToString().ToLower();
                    if ((manufacturer == "microsoft corporation" && obj["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) ||
                        manufacturer.Contains("vmware") || manufacturer.Contains("virtualbox"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckForCommonCrackTools()
        {
            string[] commonCrackTools = { "ollydbg", "ida", "windebug", "cheat", "crack", "hack", "unpack", "unpacked", "patch", "exeinfo" };
            foreach (string tool in commonCrackTools)
            {
                if (Process.GetProcessesByName(tool).Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, out bool isDebuggerPresent);
    }
}