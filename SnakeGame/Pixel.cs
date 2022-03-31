using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace SnakeGame
{
    public readonly struct Pixel
    {
        private const char PixelForBorder = '█';
        public int X { get;  }
        public int Y { get; }
        public int PixelSize { get; }
        public ConsoleColor Color { get; }
        public Pixel(int x,int y, ConsoleColor color, int pixelSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }

        public void Draw()
        {
            Console.ForegroundColor = Color;
            for (int i = 0; i < PixelSize; i++)
            {
                for (int j = 0; j < PixelSize; j++)
                {
                    SetCursorPosition(X * PixelSize + i, Y * PixelSize + j);
                    Write(PixelForBorder);
                }
            }

            
        }
        public void Clear()
        {
            for (int i = 0; i < PixelSize; i++)
            {
                for (int j = 0; j < PixelSize; j++)
                {
                    SetCursorPosition(X * PixelSize + i, Y * PixelSize + j);
                    Write(' ');
                }
            }
           
        }
    }
}
