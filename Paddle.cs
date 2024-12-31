using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Paddle
    {
        public int X_Position { get; set; }
        public int Y_Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Paddle(int x, int y, int width, int height)
        {
            X_Position = x;
            Y_Position = y;
            Width = width;
            Height = height;
        }

        // Move the paddle up or down
        public void Move(int speed)
        {
            Y_Position += speed;
            // Prevent the paddle from moving out of bounds
            if (Y_Position < 0) Y_Position = 0;
            if (Y_Position > 400) Y_Position = 400; // Assuming the form height is 450
        }

        // Draw the paddle on the form
        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.White, X_Position, Y_Position, Width, Height);
        }
    }
}
