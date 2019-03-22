using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string PlayerName { get; set; }
        public string PuzzleImageURL { get; set; }
        public ObservableCollection<ImageBrush> Brushes { get; set; }
        public GameUC()
        {
            InitializeComponent();
            Brushes = new ObservableCollection<ImageBrush>();
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
            }
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
            if (isTruePiece)
            {
                return isTruePiece;
            }
            else
            {
                trueCount = 0;
                return isTruePiece;
            }
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
                var puzzlePiece = puzzlePieces[newSelectedPiece];
                puzzlePieces[newSelectedPiece] = puzzlePieces[selectedPiece.Value];
                puzzlePieces[selectedPiece.Value] = puzzlePiece;
                Brushes[selectedPiece.Value] = GetImageBrush(puzzlePieces[selectedPiece.Value]);
                Brushes[newSelectedPiece] = GetImageBrush(puzzlePieces[newSelectedPiece]);
                if(IsSameImage(originalPuzzlePieces[newSelectedPiece], puzzlePieces[newSelectedPiece]))
                {
                    puzzleButtons[newSelectedPiece].IsEnabled = false;
                }
                if (IsSameImage(originalPuzzlePieces[selectedPiece.Value], puzzlePieces[selectedPiece.Value]))
                {
                    puzzleButtons[selectedPiece.Value].IsEnabled = false;
                }
                selectedPiece = null;
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
