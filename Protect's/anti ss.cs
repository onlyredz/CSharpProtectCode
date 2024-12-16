using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace onlyredzv2
{
    public class AntiCmdAndExternalApps
    {
        public static void ProtectApp()
        {
            // Check if Command Prompt (cmd) is running
            if (IsProcessRunning("cmd"))
            {
                MessageBox.Show("Command Prompt (cmd) execution has been detected. The application will close.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }

            // Prevent the application from being opened by another process
            Process currentProcess = Process.GetCurrentProcess();
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                if (process.Id != currentProcess.Id && process.MainModule.FileName == currentProcess.MainModule.FileName)
                {
                    MessageBox.Show($"An attempt to open the application from another process ({process.ProcessName}) has been detected. The application will close.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Environment.Exit(0);
                }
            }
        }

        private static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }
    }
}

// _______________________________________________________________

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

// Invoke protection mechanism
AntiCmdAndExternalApps.ProtectApp();
