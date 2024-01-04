using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SteamCMD_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SteamInstaller installer;
        WorkshopInstaller workshopInstaller;

        Regex rgx = new Regex("[0-9]+");

        public MainWindow()
        {
            //MainWindow.
            installer = new SteamInstaller(this);
            workshopInstaller = new WorkshopInstaller(this);

            InitializeComponent();
            //string[] arr = { "1372003680", "3071298014" };
            //workshopInstaller.DownloadOther(arr);
        }

        private void Install_SteamCMD_Button_Click(object sender, RoutedEventArgs e)
        {
            installer.DownloadSteacmCMD();
        }

        private void Download_ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void AppId_Text_Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = AppId_Text_Input.Text;
            if(ulong.TryParse(input, out ulong value)) 
            {
                workshopInstaller.SetGetGameAppId(value);
            }
        }

        private void Game_Name_Dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem? item = Game_Name_Dropdown.SelectedItem as ComboBoxItem;
            
            if(item != null)
            {
                workshopInstaller.SetGetGameAppId(item.Content.ToString());
            }
        }

        private void Links_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = Links_TextBox.Text;
            MatchCollection matches = rgx.Matches(input);

            if(matches.Count > 0)
            {
                List<string> ids = new List<string>();
                foreach(Match match in matches)
                {
                    ids.Add(match.Value);
                }

                workshopInstaller.SetWorkshopItems(ids);
            }
        }

        // ON CLICK
        private void Console_CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            // THIS DOESNT WORK, FIX IT
            bool value = (bool)Console_CheckBox.IsChecked;
            Debug.WriteLine(value);
            if (value)
            {
                workshopInstaller.SetConsoleEnabled(value);
                return;
            }
            workshopInstaller.SetConsoleEnabled(false);
        }

        private void Console_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Download_Workshop_Addons_Click(object sender, RoutedEventArgs e)
        {
            workshopInstaller.DownloadWorkshopItems();
        }

        private void Quit_CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            
            bool value = (bool) Quit_CheckBox.IsChecked;
            Debug.WriteLine(value);
            if(value)
            {
                workshopInstaller.SetQuitEnabled(value);
                return;
            }
            workshopInstaller.SetQuitEnabled(false);
            
        }

    }
}
