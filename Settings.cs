using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeG
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    public class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static Direction direction { get; set; }

        public Settings()
        {
            Width = 6;
            Height = 6;
            Speed = 16;
            Score = 0;
            Points = 100;
            GameOver = false;
            direction = Direction.Right;
        }
    }
}
