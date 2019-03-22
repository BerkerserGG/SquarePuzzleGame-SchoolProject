using Microsoft.Win32;
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

namespace SquarePuzzleGame_SchoolProject
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public event CreateNewGame NewGameEvent;
        public MainMenu()
        {
            InitializeComponent();
        }

        private void NewGame_Button_Click(object sender, RoutedEventArgs e)
        {
            var brush = new ImageBrush();
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            PlayerNameDialog nameDialog = new PlayerNameDialog();
            nameDialog.Owner = mainWindow;
            if(nameDialog.ShowDialog() == true)
            {
                OpenFileDialog o_fileDialog = new OpenFileDialog();
                o_fileDialog.Multiselect = false;
                o_fileDialog.Filter = "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                    "PNG (*.png)|*.png|" +
                    "Bütün Resim Dosyaları|*.jpg;*.jpeg;*.png";
                // TODO i am here
                if (o_fileDialog.ShowDialog() == true)
                {
                    mainWindow.PuzzleImageURL = o_fileDialog.FileName;
                    NewGameEvent();
                }
            }
        }
    }
}
