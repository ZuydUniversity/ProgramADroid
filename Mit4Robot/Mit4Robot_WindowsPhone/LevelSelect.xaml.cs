using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Mit4Robot_WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelSelect : Page
    {
        public LevelSelect()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            LoadLevelsList("Easy");
        }

        private void cbLevelPack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLevelPack != null)
            {
                var selected = ((ComboBoxItem)cbLevelPack.SelectedItem).Content.ToString();
                //if (cbLevelPack.SelectedItem)
                LoadLevelsList(selected);
            }
        }

        private void cbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lbLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = lbLevels.SelectedItem.ToString();
            Frame.Navigate(typeof(Game), selected);
        }

        private async void LoadLevelsList(string difficulty)
        {
            lbLevels.Items.Clear(); // Clear list before doing anything
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var levels = await folder.GetFolderAsync(@"Assets\Levels\" + difficulty);

            foreach (var file in await levels.GetFilesAsync())
            {
                lbLevels.Items.Add(file.Name);
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
    }
}
