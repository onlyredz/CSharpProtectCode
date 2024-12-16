using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace AntiCmdApp
{
    public partial class Form1 : Form
    {
        private const string cmdProcessName = "cmd";
        private const int detectionInterval = 1000; 

        private bool cmdDetected = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Thread detectionThread = new Thread(new ThreadStart(DetectCmd));
            detectionThread.IsBackground = true;
            detectionThread.Start();
        }

        private void DetectCmd()
        {
            while (true)
            {
               
                Process[] processes = Process.GetProcessesByName(cmdProcessName);

               
                if (processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        try
                        {
                            process.Kill(); 
                            cmdDetected = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao fechar o prompt de comando: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                Thread.Sleep(detectionInterval);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            if (cmdDetected)
            {
                MessageBox.Show("O prompt de comando (cmd.exe) foi detectado e fechado.", "Informações", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}