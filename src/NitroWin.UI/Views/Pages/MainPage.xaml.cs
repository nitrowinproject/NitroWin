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
using NitroWin.UI.Views.Windows;

namespace NitroWin.UI.Views.Pages {
    public partial class MainPage : Page {
        public MainPage() {
            InitializeComponent();
        }
        public void AboutButtonClick(object sender, RoutedEventArgs e) {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ContentFrame.Navigate(new AboutPage());
            }
        }
        public void SelectConfigFileButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
