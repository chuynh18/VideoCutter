using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using VideoCutter.HelperClasses;

namespace VideoCutter
{
    /// <summary>
    /// Interaction logic for Extractor.xaml
    /// </summary>
    public partial class Extractor : UserControl
    {
        public Extractor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allows user to select an input video via dialog box.
        /// The path to the chosen video is placed into the Input_Video TextBox.
        /// A suggested filename is placed into the Output_Video_name TextBox.
        /// </summary>
        private void Select_Video(object sender, RoutedEventArgs e)
        {
            SelectFileHelper.Select_Video(UpdateUIAfterVideoSelected);
        }

        private void Select_Output_Folder(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateUIAfterVideoSelected(string path)
        {
            Input_Video.Text = path;
        }
    }
}
