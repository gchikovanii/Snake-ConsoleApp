using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace SnakeGame
{
    internal class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;

        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeight = MapHeight * 3;
        private const int FrameInMs = 200;
        private static readonly Random Random = new Random();

        private const ConsoleColor BorderColor = ConsoleColor.DarkRed;
        private const ConsoleColor HeadColor = ConsoleColor.Gray;
        private const ConsoleColor BodyColor = ConsoleColor.DarkBlue;
        private const ConsoleColor FoodColor = ConsoleColor.DarkYellow;

        

        static void Main(string[] args)
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;
            while (true)
            {
                StartGame();
                Thread.Sleep(1000);

                ReadKey();
            }
           
            
        }

        #region Generate Food
        public static Pixel GenerateFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(Random.Next(1,MapWidth - 2),Random.Next(MapHeight -2),FoodColor);
            } 
            while (snake.Head.X == food.X && snake.Head.Y == food.Y 
            || snake.Body.Any(i => i.X == food.X && i.Y == food.Y));

            return food;
        }
        #endregion
        #region Starting Game 
        public static void StartGame()
        {
            Clear();
            DrawBorder();

            int score = 0;
            //On low processors to hold same speed
            int lagInMs = 0;
            var snake = new Snake(10, 5, HeadColor, BodyColor);
            Pixel food = GenerateFood(snake);
            food.Draw();
            Stopwatch stopwatch = new Stopwatch();
            Direction currentMovement = Direction.Right;
            while (true)
            {
                stopwatch.Restart();
                //Snake cant move in different directions in one shot
                Direction oldMovement = currentMovement;
                while (stopwatch.ElapsedMilliseconds <= FrameInMs - lagInMs)
                {
                    if (currentMovement == oldMovement)
                        currentMovement = ReadMovement(currentMovement);
                }
                stopwatch.Restart();
                if(snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);

                    food = GenerateFood(snake);
                    score++;
                    food.Draw();
                    Task.Run(() => Beep(1800, 300));
                    if(score == 35)
                    {
                        break;
                    }
                }
                snake.Move(currentMovement);

                if (snake.Head.X == MapWidth - 1 || snake.Head.Y == MapHeight - 1 || snake.Head.Y == 0
                    || snake.Body.Any(i => i.X == snake.Head.X && i.Y == snake.Head.Y))
                    break;
                lagInMs = Convert.ToInt32(stopwatch.ElapsedMilliseconds);
            }
            snake.Clear();
            SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
            if(score == 35)
            {
                snake.Clear();
                SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
                WriteLine($"You have won the game! Your Total score was: {score}");
                Task.Run(() => Beep(400, 500));
            }
            else
            {
                snake.Clear();
                SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
                WriteLine($"Game Over! Your Total score was: {score}");
                Task.Run(() => Beep(400, 500));
            }

            

        }
        #endregion
        #region Movment
        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;
            ConsoleKey key = ReadKey(true).Key;

            //User cant move left and right at the same time!
            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                _ => currentDirection
            };
            return currentDirection;
        }
        #endregion
        #region DrawBorder
        static void DrawBorder()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHeight - 1 , BorderColor).Draw();
            }
            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0,i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }
        #endregion
    }
}
