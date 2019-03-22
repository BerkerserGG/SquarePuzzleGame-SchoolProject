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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public delegate void CreateNewGame();
    public partial class MainWindow : Window
    {
        private MainMenu mainUC = new MainMenu();
        public string PlayerName { get; set; }
        public string PuzzleImageURL { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            PlayerName = null;
            PuzzleImageURL = null;
            mainUC.NewGameEvent += OpenGameUC;
            MainContent.Content = mainUC;
        }

        private void OpenGameUC()
        {
            GameUC gameUC = new GameUC();
            gameUC.PlayerName = PlayerName;
            gameUC.PuzzleImageURL = PuzzleImageURL;
            MainContent.Content = gameUC;
        }
    }
}
