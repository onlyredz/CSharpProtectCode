using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

public static class AntiTampering
{
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

    public static bool IsDebuggerPresent()
    {
        bool isDebuggerPresent = false;
        CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
        return isDebuggerPresent;
    }

    public static bool IsRunningUnderDebugger()
    {
        return Debugger.IsAttached || IsDebuggerPresent();
    }

    public static bool IsElevated()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static bool IsScanningToolRunning()
    {
        string[] scanningTools = { "cheatengine", "ollydbg", "ida", "x64dbg" }; // List of known scanning tools

        foreach (string tool in scanningTools)
        {
            Process[] processes = Process.GetProcessesByName(tool);
            if (processes.Length > 0)
            {
                Console.WriteLine("A scanning tool is currently running: " + tool);
                return true;
            }
        }

        return false;
    }
}

// ____________________________________________________

if (AntiTampering.IsRunningUnderDebugger())
{
    Console.WriteLine("The application is running under a debugger.");
}

if (AntiTampering.IsScanningToolRunning())
{
    Console.WriteLine("A scanning tool has been detected running.");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
