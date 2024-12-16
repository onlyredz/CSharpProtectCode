using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace onlyredzv2
{
    public class AntiDllInjection
    {
        // Import kernel32.dll to check if a debugger is present
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool IsDebuggerPresent();

        // Import kernel32.dll to check for remote debugger presence
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        // Check if a local debugger is attached
        public static bool IsDebuggerAttached()
        {
            return IsDebuggerPresent();
        }

        // Check if a remote debugger is attached to the given process
        public static bool IsRemoteDebuggerAttached(Process process)
        {
            bool isDebuggerPresent = false;
            CheckRemoteDebuggerPresent(process.Handle, ref isDebuggerPresent);
            return isDebuggerPresent;
        }

        // Check for DLL injection by inspecting loaded modules
        public static bool IsDllInjected()
        {
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                if (!module.FileName.StartsWith(AppDomain.CurrentDomain.BaseDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        // Protect the application by detecting debuggers and DLL injections
        public static void ProtectApp()
        {
            if (IsDebuggerAttached() || IsDllInjected())
            {
                MessageBox.Show("The presence of a debugging DLL or DLL injection has been detected in the current process. The application will close.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }
        }
    }
}

// __________________________________________________

// Initialize application settings
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

// Protect the application from debugging or DLL injection
AntiDllInjection.ProtectApp();
