using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args)
    {
        // Get the path of the executable
        string exePath = typeof(Program).Assembly.Location;
        string exeHash = CalculateMD5(exePath);

        // Verify the digital signature of the executable
        X509Certificate2 cert = new X509Certificate2(exePath);
        bool isSignatureValid = cert.Verify();

        Console.WriteLine("MD5 hash of the executable file: " + exeHash);
        Console.WriteLine("Valid digital signature: " + isSignatureValid);
    }

    static string CalculateMD5(string filePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}

// Calculate the MD5 hash of your own executable

// ___________________________________________________

using System;
using System.IO;
using System.Security.Cryptography;

public class HashCalculator
{
    public static string CalculateMD5(string filePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                var hashBytes = md5.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
