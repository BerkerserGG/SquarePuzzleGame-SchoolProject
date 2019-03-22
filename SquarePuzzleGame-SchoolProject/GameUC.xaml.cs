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
    /// Interaction logic for GameUC.xaml
    /// </summary>
    public partial class GameUC : UserControl
    {
        private bool isRandomFirstTime = true;
        private BitmapSource[] originalPuzzlePieces = new BitmapSource[16];
        private BitmapSource[] puzzlePieces = new BitmapSource[16];
        private Button[] puzzleButtons = new Button[16];
        private int? selectedPiece = null;
        private int trueCount = 0;
        private int maxMove;
        private int moveCount = 0;
        public ObservableCollection<string> PlayerName { get; set; }
        public string PuzzleImageURL { get; set; }
        public ObservableCollection<ImageBrush> Brushes { get; set; }
        public ObservableCollection<double> Score { get; set; }
        public event ReturnMainMenu EndGameEvent;
        public GameUC()
        {
            InitializeComponent();
            Brushes = new ObservableCollection<ImageBrush>();
            PlayerName = new ObservableCollection<string>();
            Score = new ObservableCollection<double>();
            puzzleButtons[0] = PieceBUtton0;
            puzzleButtons[1] = PieceBUtton1;
            puzzleButtons[2] = PieceBUtton2;
            puzzleButtons[3] = PieceBUtton3;
            puzzleButtons[4] = PieceBUtton4;
            puzzleButtons[5] = PieceBUtton5;
            puzzleButtons[6] = PieceBUtton6;
            puzzleButtons[7] = PieceBUtton7;
            puzzleButtons[8] = PieceBUtton8;
            puzzleButtons[9] = PieceBUtton9;
            puzzleButtons[10] = PieceBUtton10;
            puzzleButtons[11] = PieceBUtton11;
            puzzleButtons[12] = PieceBUtton12;
            puzzleButtons[13] = PieceBUtton13;
            puzzleButtons[14] = PieceBUtton14;
            puzzleButtons[15] = PieceBUtton15;
            DataContext = this;
        }

        private void Random_Button_Click(object sender, RoutedEventArgs e)
        {
            if (isRandomFirstTime)
            {
                PreparePuzzlePieces();
                isRandomFirstTime = false;
                RandomizePieces();
            }
            else
            {
                Brushes.Clear();
                RandomizePieces();
            }
            foreach (var button in puzzleButtons)
            {
                button.IsEnabled = true;
            }
            if (RandomControl())
            {
                Button randomButton = sender as Button;
                randomButton.IsEnabled = false;
                StartScore();
                WinCondition();
            }
            else
            {
                foreach (var button in puzzleButtons)
                {
                    button.IsEnabled = false;
                }
            }
        }
        private void WinCondition()
        {
            if(trueCount == 16)
            {
                MessageBox.Show("Kazandınız");
                EndGame();
                EndGameEvent();
            }
        }
        private void EndGame()
        {
            List<PlayerScore> scoreList = new List<PlayerScore>();
            using(FileStream fs = new FileStream("enyüksekskor.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                using(StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string line;
                    while((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(' ');
                        scoreList.Add(new PlayerScore(data[0], double.Parse(data[1])));
                    }
                }
                scoreList.Add(new PlayerScore(PlayerName[0], Score[0]));
            }
            using (FileStream fs = new FileStream("enyüksekskor.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    List<PlayerScore> sortedScoreList = scoreList.OrderByDescending(obj => obj.Score).ToList();
                    foreach (var item in sortedScoreList)
                    {
                        string line = item.PlayerName + " " + item.Score;
                        sw.WriteLine(line);
                    }
                }
            }
        }
        private void StartScore()
        {
            maxMove = 15 - trueCount;
            Score.Add(trueCount * 6.25);
        }
        private bool RandomControl()
        {
            bool isTruePiece = false;
            for (int i = 0; i < 16; i++)
            {
                if(IsSameImage(originalPuzzlePieces[i], puzzlePieces[i]))
                {
                    isTruePiece = true;
                    trueCount++;
                    puzzleButtons[i].IsEnabled = false;
                }
            }
            return isTruePiece;
        }
        private void PreparePuzzlePieces()
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(PuzzleImageURL, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            int pieceHeigth = src.PixelHeight / 4;
            int pieceWidth = src.PixelWidth / 4;
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    originalPuzzlePieces[count++] = new CroppedBitmap(src, new Int32Rect(j * pieceWidth, i * pieceHeigth, pieceWidth, pieceHeigth));
                }
            }
        }
        private void RandomizePieces()
        {
            List<BitmapSource> puzzlePieceList = new List<BitmapSource>();
            foreach (BitmapSource puzzlePiece in originalPuzzlePieces)
            {
                puzzlePieceList.Add(puzzlePiece);
            }
            int size = 16;
            Random random = new Random();
            for (int i = 0; i < 16; i++)
            {
                int index = random.Next(size--);
                BitmapSource piece = puzzlePieceList[index];
                puzzlePieces[i] = piece;
                Brushes.Add(GetImageBrush(piece));
                puzzlePieceList.Remove(piece);
            }
            foreach (var button in puzzleButtons)
            {
                button.IsEnabled = true;
            }
        }
        private bool IsSameImage(BitmapSource img1, BitmapSource img2)
        {
            byte[] img1Data = GetImagePixelData(img1);
            byte[] img2Data = GetImagePixelData(img2);
            if(img1Data.Length == img2Data.Length)
            {
                for (int i = 0; i < img2Data.Length; i++)
                {
                    if(img1Data[i] != img2Data[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        private byte[] GetImagePixelData(BitmapSource img)
        {
            int stride = img.PixelWidth * 4;
            int size = img.PixelHeight * stride;
            byte[] pixelData = new byte[size];
            img.CopyPixels(pixelData, stride, 0);
            return pixelData;
        }

        private void Piece_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int newSelectedPiece = int.Parse(button.Tag.ToString());
            if (selectedPiece.HasValue)
            {
                if(newSelectedPiece == selectedPiece.Value)
                {
                    selectedPiece = null;
                    return;
                }
                var puzzlePiece = puzzlePieces[newSelectedPiece];
                puzzlePieces[newSelectedPiece] = puzzlePieces[selectedPiece.Value];
                puzzlePieces[selectedPiece.Value] = puzzlePiece;
                Brushes[selectedPiece.Value] = GetImageBrush(puzzlePieces[selectedPiece.Value]);
                Brushes[newSelectedPiece] = GetImageBrush(puzzlePieces[newSelectedPiece]);
                bool isMistake = true;
                if(IsSameImage(originalPuzzlePieces[newSelectedPiece], puzzlePieces[newSelectedPiece]))
                {
                    isMistake = false;
                    puzzleButtons[newSelectedPiece].IsEnabled = false;
                    Score[0] += 6.25;
                    trueCount++;
                }
                if (IsSameImage(originalPuzzlePieces[selectedPiece.Value], puzzlePieces[selectedPiece.Value]))
                {
                    isMistake = false;
                    puzzleButtons[selectedPiece.Value].IsEnabled = false;
                    Score[0] += 6.25;
                    trueCount++;
                }
                if (isMistake)
                {
                    Score[0] -= 12.5;
                }
                if(++moveCount > maxMove)
                {
                    Score[0] -= 6.25;
                }
                selectedPiece = null;
                WinCondition();
            }
            else
            {
                selectedPiece = newSelectedPiece;
            }
        }
        private ImageBrush GetImageBrush(BitmapSource img)
        {
            var brush = new ImageBrush();
            brush.ImageSource = img;
            return brush;
        }
    }
}
