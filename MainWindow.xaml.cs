using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // static variable to hold reference to the main window
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            // after initialization, set static variable Instance to the main window
            Instance = this;
        }

        private void Cutter_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public static void ChangeActiveTab(int tabIndex)
        {
            if (Instance != null)
            {
                Instance.MainWindowTabControl.SelectedIndex = tabIndex;
            }
        }
    }
}
