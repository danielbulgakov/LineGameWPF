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
using System.Windows.Threading;


namespace LineGame_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int CountStart = 5;
        
        int IntroTime = 200;
        int GameTime = 1000;

        bool IntroNotOver = true; // Для быстродействия обработчика клика объекта
        bool lose = false;

        DispatcherTimer Timer;
        
        
        public MainWindow()
        {
            InitializeComponent();
            CreateLines();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(IntroTime);
            Timer.Tick += new EventHandler(GameControl);
            Timer.Start();
            
        }

        private void GameControl(object sender, EventArgs e)
        {
            if (GameGrid.Children.Count == 1)
            {
                Timer.Stop();
                txtResult.Text = "You Win!";
                txtResult.Visibility = Visibility.Visible;
                
            }
            else CreateLines();
            if (GameGrid.Children.Count == 2 * CountStart)
            {
                Timer.Stop();
                txtResult.Text = "Game over!";
                
                txtResult.Visibility = Visibility.Visible;
                lose = true;
            }
            



            if (GameGrid.Children.Count >= CountStart + 1)
            {
                Timer.Interval = TimeSpan.FromMilliseconds(GameTime);
                IntroNotOver = false;
            }

            


        }

        private void CreateLines()
        {
            Rectangle Line = new Rectangle();
            Line.Width = 230;
            Line.Height = 35;
            int X, Y;
            Random CRandom = new Random();
            Brush brush = new SolidColorBrush(Color.FromRgb((byte)CRandom.Next(1, 255),
              (byte)CRandom.Next(1, 255), (byte)CRandom.Next(1, 233)));
            Line.Fill = brush;
            Random IsHorizontal = new Random();
            Random Pos = new Random();
            if (IsHorizontal.Next(0, 2) == 1)
            {
                (Line.Width, Line.Height) = (Line.Height, Line.Width);
                X = Pos.Next(30, Convert.ToInt32(this.Width - Line.Width - Line.Height));
                Y = Pos.Next(30, Convert.ToInt32(this.Height - Line.Width - Line.Height));
            }
            else
            {
                X = Pos.Next(30, Convert.ToInt32(this.Width - Line.Width - Line.Height));
                Y = Pos.Next(30, Convert.ToInt32(this.Height - Line.Width - Line.Height));
            }
            Line.HorizontalAlignment = HorizontalAlignment.Left;
            Line.VerticalAlignment = VerticalAlignment.Top;
            Line.Margin = new Thickness(X, Y, 0, 0);
            Line.Stroke = new SolidColorBrush(Colors.Black);
            Line.MouseLeftButtonDown += new MouseButtonEventHandler(LineClicked);
            GameGrid.Children.Add(Line);                      

        }

        private void LineClicked (object sender, MouseButtonEventArgs e)
        {
            Rectangle Line = (Rectangle)sender;
            Rect RPos = new Rect(Line.Margin.Left, Line.Margin.Top, Line.Width, Line.Height);
            int Susline = GameGrid.Children.IndexOf(Line);
            var LL = GameGrid.Children.OfType<Rectangle>().ToList();

            foreach(Rectangle _Line in LL)
            {
                Rect ToCheck = new Rect(_Line.Margin.Left, _Line.Margin.Top, _Line.Width, _Line.Height);
                if (RPos.IntersectsWith(ToCheck))
                {
                    if (GameGrid.Children.IndexOf(Line) < GameGrid.Children.IndexOf(_Line))                        
                        return;
                    if (lose) return;
                    if (IntroNotOver) return;
                    
                    
                }
            }
            this.GameGrid.Children.Remove((UIElement)sender);
            return;                
        }

        
    }
}
