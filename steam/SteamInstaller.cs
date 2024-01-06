using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Windows;
using SteamCMD_Tools.steam;

namespace SteamCMD_Tools
{
    internal class SteamInstaller
    {
        private MainWindow window;

        private const string CMD_DOWNLOAD_URL = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
        private const string CMD_ZIP_FILE = "steamcmd.zip";
        private const string CMD_DIR = "steam_cmd";
        private const string CMD_FILENAME = "steamcmd.exe";

        private bool isInstalling;

        public SteamInstaller(MainWindow window)
        {
            this.window = window;
            this.isInstalling = false;
        }

        public void DownloadSteacmCMD()
        {
            if (isInstalling)
                return;

            try
            {
                isInstalling = true;
                WebClient client = new WebClient();
                Uri url = new Uri(CMD_DOWNLOAD_URL);


                client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
                client.DownloadFileCompleted += Client_DownloadFileCompleted;


                client.DownloadFileAsync(url, CMD_ZIP_FILE);
            }
            catch (WebException ex)
            {
                // Do nothing
            }
        }

        private void Client_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            InstallSteamCMD();
        }

        private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage <= 70) { }
              window.Download_ProgressBar.Value = e.ProgressPercentage;
        }

        public async void InstallSteamCMD()
        {
            window.GetSettings().CheckIfInstalled();

            if (window.GetSettings().IsInstalled())
            {
                MessageBox.Show("Already installed!", "SteamCMD Tools",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
                

            Directory.CreateDirectory(CMD_DIR);
            ZipFile.ExtractToDirectory(CMD_ZIP_FILE, CMD_DIR, true);

            using (Process process = new Process())
            {
                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process.StartInfo.FileName = $"{CMD_DIR}/{CMD_FILENAME}";
                process.StartInfo.Arguments = "+quit";
                process.StartInfo.UseShellExecute = false;
                //process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = false;
                //process.OutputDataReceived += Process_OutputDataReceived;

                process.Start();


                Task task = process.WaitForExitAsync();
                task.Wait();

                process.CloseMainWindow();
                process.Close();

                window.Download_ProgressBar.Value = 100;

                MessageBox.Show("Installation completed!", "SteamCMD Tools",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                isInstalling = false;
                window.GetSettings().CheckIfInstalled();

            }

        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
           // Debug.WriteLine(e.Data);
        }
    }
}
