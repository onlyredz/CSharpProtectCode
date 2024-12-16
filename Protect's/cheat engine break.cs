using System;
using System.Diagnostics;
using System.IO;

public class AntiCheatEngine
{
    public static bool IsCheatEngineRunning()
    {
        // Determine the system architecture
        string architecture = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";

        // Get all running processes
        Process[] processes = Process.GetProcesses();

        // Check each process for Cheat Engine presence
        foreach (Process process in processes)
        {
            if (IsCheatProcess(process.ProcessName) || IsCheatModuleLoaded(process.Id))
            {
                return true; 
            }
        }

        return false; 
    }

    private static bool IsCheatProcess(string processName)
    {
        // Check if the process name contains "cheat" or "engine"
        return processName.ToLower().Contains("cheat") || processName.ToLower().Contains("engine");
    }

    private static bool IsCheatModuleLoaded(int processId)
    {
        try
        {
            // Get the process by its ID and check its modules
            Process process = Process.GetProcessById(processId);
            foreach (ProcessModule module in process.Modules)
            {
                // Check if the module name contains "cheat" or "engine"
                if (module.ModuleName.ToLower().Contains("cheat") || module.ModuleName.ToLower().Contains("engine"))
                {
                    return true; 
                }
            }
        }
        catch (Exception)
        {
            // Ignore any errors when accessing the process modules
        }
        
        return false; 
    }
}

// ____________________________________

if (AntiCheatEngine.IsCheatEngineRunning())
{
    Console.WriteLine("Cheat Engine detected. Terminating the application...");
    return;
}

Console.WriteLine("The application is running without detecting Cheat Engine.");
