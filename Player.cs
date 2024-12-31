using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Player
    {
        public Paddle Paddle { get; set; }
        public int Score { get; set; }
        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }

        public Player(int xPosition)
        {
            Paddle = new Paddle(xPosition, 150, 10, 100); // You can customize paddle size and position
            Score = 0;
            MoveUp = false;
            MoveDown = false;
        }

        public void UpdateMovement()
        {
            if (MoveUp)
                Paddle.Move(-5);
            if (MoveDown)
                Paddle.Move(5);
        }
    }
}
