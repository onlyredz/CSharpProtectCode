using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntiCmd
{
    public static class CmdBlocker
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static void BlockCmdPrompt()
        {
            string cmdWindowTitle = "Command Prompt";

            IntPtr foregroundWindowHandle = GetForegroundWindow();
            StringBuilder windowTitle = new StringBuilder(256);
            GetWindowText(foregroundWindowHandle, windowTitle, windowTitle.Capacity);

            if (windowTitle.ToString().Contains(cmdWindowTitle))
            {
                MessageBox.Show("O uso do prompt de comando não é permitido enquanto este aplicativo está em execução.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Process.GetCurrentProcess().Kill();
            }
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CmdBlocker.BlockCmdPrompt();
        }
    }
}