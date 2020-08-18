using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoCutter
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
        }

        private void Uri_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            if (hyperlink != null)
            {
                Process.Start(hyperlink.NavigateUri.AbsoluteUri);
            }
        }

        private void Settings_Text_Clicked(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeActiveTab(3);
        }
    }
}
