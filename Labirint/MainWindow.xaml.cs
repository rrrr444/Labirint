using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Labirint
{
    public class Player
    {
        public Rectangle Rectangle { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; } = 20;

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            Rectangle = new Rectangle
            {
                Width = Size,
                Height = Size,
                Fill = Brushes.Blue
            };
        }

        public void Move(int deltaX, int deltaY)
        {
            X += deltaX;
            Y += deltaY;
            Canvas.SetLeft(Rectangle, X * Size);
            Canvas.SetTop(Rectangle, Y * Size);
        }
    }

    public class Maze
    {
        private int[,] layout;
        public int CellSize { get; set; } = 20;

        public Maze(int[,] layout)
        {
            this.layout = layout;
        }

        public void Draw(Canvas canvas)
        {
            canvas.Children.Clear();

            for (int x = 0; x < layout.GetLength(0); x++)
            {
                for (int y = 0; y < layout.GetLength(1); y++)
                {
                    if (layout[x, y] == 1) // Стена
                    {
                        Rectangle wall = new Rectangle
                        {
                            Width = CellSize,
                            Height = CellSize,
                            Fill = Brushes.Black
                        };
                        Canvas.SetLeft(wall, x * CellSize);
                        Canvas.SetTop(wall, y * CellSize);
                        canvas.Children.Add(wall);
                    }
                    else if (layout[x, y] == 2) // Выход
                    {
                        Rectangle exit = new Rectangle
                        {
                            Width = CellSize,
                            Height = CellSize,
                            Fill = Brushes.Green
                        };
                        Canvas.SetLeft(exit, x * CellSize);
                        Canvas.SetTop(exit, y * CellSize);
                        canvas.Children.Add(exit);
                    }
                }
            }
        }

        public bool IsWall(int x, int y)
        {
            // Проверка границ массива
            if (x < 0 || x >= layout.GetLength(0) || y < 0 || y >= layout.GetLength(1))
                return true; // Если за пределами лабиринта, считаем это стеной

            return layout[y, x] == 1;
        }

        public bool IsExit(int x, int y)
        {
            return layout[y, x] == 2;
        }
    }

    public partial class MainWindow : Window
    {
        private Player player;
        private Maze maze;

        private int[,] mazeLayout = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 1, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 2, 1 }, // 2 - выход
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        public MainWindow()
        {
            InitializeComponent();
            player = new Player(1, 1);
            maze = new Maze(mazeLayout);
            maze.Draw(GameCanvas);
            GameCanvas.Children.Add(player.Rectangle);
            Canvas.SetLeft(player.Rectangle, player.X * player.Size);
            Canvas.SetTop(player.Rectangle, player.Y * player.Size);
            StatusText.Text = "Найдите выход!";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int newX = player.X;
            int newY = player.Y;

            switch (e.Key)
            {
                case Key.Up:
                    newY--;
                    break;
                case Key.Down:
                    newY++;
                    break;
                case Key.Left:
                    newX--;
                    break;
                case Key.Right:
                    newX++;
                    break;
            }

            // Проверка на выход
            if (maze.IsExit(newX, newY))
            {
                StatusText.Text = "Вы выиграли! Поздравляем!";
                return;
            }

            // Проверка на столкновение со стеной
            if (!maze.IsWall(newX, newY))
            {
                player.Move(newX - player.X, newY - player.Y);
            }
        }
    }
}
