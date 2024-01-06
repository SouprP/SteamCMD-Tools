using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SteamCMD_Tools.steam
{
    public class Settings
    {
        private bool consoleEnabled;
        private bool quitEnabled;
        private bool isInstalled;

        private const string CMD_DIR = "steam_cmd";

        public Settings()
        {
            this.consoleEnabled = true;
            this.quitEnabled = true;
            this.isInstalled = false;
        }

        public void SetConsoleEnabled(bool consoleEnabled)
        {
            this.consoleEnabled = consoleEnabled;
        }

        public bool IsConsoleEnabled()
        {
            return this.consoleEnabled;
        }

        public void SetQuitEnabled(bool quitEnabled)
        {
            this.quitEnabled = quitEnabled;
        }

        public bool IsQuitEnabled()
        {
            return this.quitEnabled;
        }

        public bool IsInstalled()
        {
            return this.isInstalled;
        }

        public void CheckIfInstalled()
        {
            if (Directory.Exists(CMD_DIR) && Directory.GetFiles(CMD_DIR).Length > 0)
            {
                this.isInstalled = true;
                return;
            }

            this.isInstalled = false;

        }
    }
}   
