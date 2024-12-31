using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Ball
    {
        public int X_Position { get; set; }
        public int Y_Position { get; set; }
        public int X_Speed;
        public int Y_Speed;
        public int Size { get; set; }

        public Ball(int x, int y, int xSpeed, int ySpeed)
        {
            X_Position = x;
            Y_Position = y;
            X_Speed = xSpeed;
            Y_Speed = ySpeed;
            Size = 10;
        }

        // Move the ball based on its speed
        public void Move()
        {
            X_Position += X_Speed;
            Y_Position += Y_Speed;
        }

        // Draw the ball on the form
        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.White, X_Position, Y_Position, Size, Size);
        }

    }
}
