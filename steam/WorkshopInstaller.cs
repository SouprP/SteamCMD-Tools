using SteamCMD_Tools.steam;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Threading.Tasks;

namespace SteamCMD_Tools
{
    internal class WorkshopInstaller
    {
        private MainWindow window;

        private const string CMD_DIR = "steam_cmd";
        private const string CMD_FILENAME = "steamcmd.exe";

        private readonly Dictionary<string, ulong> GAME_APP_ID
            = new Dictionary<string, ulong>()
            {
                {"Rimworld", 294100 },
                {"Cities Skylines", 255710 },
                {"Hearts of Iron IV", 394360 },
                {"Project Zomboid", 108600 },
                {"Garry's Mod", 4000},
                {"Crusader Kings III", 1158310 },
                {"CS:GO", 624820 }
            };

        
        private ulong AppId = 0;
        private List<string> workshopContentIds;
        // private bool consoleEnabled;
        // private bool quitEnabled;

        public WorkshopInstaller(MainWindow window) 
        {
            this.window = window;
            this.workshopContentIds = new List<string>();
            //this.consoleEnabled = true;
            //this.quitEnabled = true;
        }

        public async void DownloadWorkshopItems()
        {
            window.GetSettings().CheckIfInstalled();

            if (!window.GetSettings().IsInstalled())
            {
                MessageBox.Show("SteamCMD not installed.", "SteamCMD Tools",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(workshopContentIds.Count == 0)
            {
                MessageBox.Show("There are no addons to download.", "SteamCMD Tools",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (Process process = new Process())
            {
                string args = "+login anonymous ";
                foreach (string item in workshopContentIds)
                    args += $"+workshop_download_item {AppId} {item} ";

                if(window.GetSettings().IsQuitEnabled())
                  args += "+quit";

                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process.StartInfo.FileName = $"{CMD_DIR}/{CMD_FILENAME}";
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.CreateNoWindow = !window.GetSettings().IsConsoleEnabled();

                //if (consoleEnabled)
                   // process.StartInfo.RedirectStandardOutput = true;

                process.Start();

                /*
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    Debug.Write(line);
                    window.Console_TextBox.AppendText(line);
                    // do something with line
                }
                */

                Task task = process.WaitForExitAsync();

                task.Wait();
                process.CloseMainWindow();
                process.Close();
            }

            using (Process explorer = new Process())
            {
                explorer.StartInfo.Arguments = Directory.GetCurrentDirectory()
                    + "\\" + CMD_DIR + "\\steamapps\\workshop\\content\\"; //+ AppId;

                explorer.StartInfo.FileName = "explorer.exe";

                explorer.Start();
            }
        }

        public void SetGetGameAppId(ulong gameId)
        {
            AppId = gameId;
        }

        public void SetGetGameAppId(string gameName)
        {
            AppId = GAME_APP_ID[gameName];
            window.AppId_Text_Input.Text = AppId.ToString();
        }

        public void SetWorkshopItems(List<string> items)
        {
            workshopContentIds = items;
        }

    }
}
