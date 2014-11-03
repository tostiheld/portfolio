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

using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace BombDefuserSetup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string ownPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            tbxPath.Text = Path.Combine(
                ownPath,
                "BombDefuser");
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ValidateNames = false;
            ofd.CheckFileExists = false;
            ofd.FileName = "Folder Selection";

            if (ofd.ShowDialog() == true)
            {
                tbxPath.Text = ofd.FileName.Substring(0, ofd.FileName.Length - 17) + "\\BombDefuser";
            }
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            btnInstall.IsEnabled = false;
            tbxPath.IsEnabled = false;
            btnBrowse.IsEnabled = false;

            using (FileStream fs = new FileStream(tbxPath.Text, FileMode.))
        }
    }
}
