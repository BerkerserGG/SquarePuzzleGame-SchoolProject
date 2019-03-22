using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Interaction logic for BestScoreUC.xaml
    /// </summary>
    public partial class BestScoreUC : UserControl
    {
        public ObservableCollection<PlayerScore> ScoreList { get; set; }
        public event ReturnMainMenu BacktoMainMenu;
        public BestScoreUC()
        {
            InitializeComponent();
            ScoreList = new ObservableCollection<PlayerScore>();
            DataContext = this;
            ReadScoreList();
        }
        private void ReadScoreList()
        {
            using(FileStream fs = new FileStream("enyüksekskor.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(' ');
                        ScoreList.Add(new PlayerScore(data[0], double.Parse(data[1])));
                    }
                }
            }
        }

        private void Return_Button_Click(object sender, RoutedEventArgs e)
        {
            BacktoMainMenu();
        }
    }
}
