using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Game
    {
        private Ball ball;
        private Player player1;
        private Player player2;
        private int paddleCollisionCount;

        public Game()
        {
            ball = new Ball(250, 200, 4, 4);
            player1 = new Player(20);
            player2 = new Player(780);
            paddleCollisionCount = 0;
        }

        // Getter for Ball's X_Speed
        public int GetBallSpeed()
        {
            return ball.X_Speed;
        }

        // Getter for Player 1
        public Player GetPlayer1()
        {
            return player1;
        }

        // Getter for Player 2
        public Player GetPlayer2()
        {
            return player2;
        }

        // Getter for Player 1's score
        public int GetPlayer1Score()
        {
            return player1.Score;
        }

        // Getter for Player 2's score
        public int GetPlayer2Score()
        {
            return player2.Score;
        }

        public void Update()
        {
            ball.Move();
            player1.UpdateMovement();
            player2.UpdateMovement();
            DetectCollisions();
        }

        public void DetectCollisions()
        {
            // Ball colliding with top or bottom wall
            if (ball.Y_Position <= 0 || ball.Y_Position >= Form1.ActiveForm.ClientSize.Height - ball.Size)
            {
                ball.Y_Speed = -ball.Y_Speed; // Reverse vertical direction
            }

            // Ball colliding with Paddles (for both players)
            if (ball.X_Speed < 0) // Moving toward player 1's paddle
            {
                DetectPaddleCollision(player1.Paddle, ref ball.X_Speed, -1);
            }
            else if (ball.X_Speed > 0) // Moving toward player 2's paddle
            {
                DetectPaddleCollision(player2.Paddle, ref ball.X_Speed, 1);
            }

            // After 3 collisions, increase xSpeed by 1
            if (paddleCollisionCount >= 3)
            {
                ball.X_Speed = ball.X_Speed < 0 ? ball.X_Speed - 1 : ball.X_Speed + 1; // Increase speed (both directions)
                paddleCollisionCount = 0; // Reset collision count
            }

            // Ball off the left or right side (score points)
            if (ball.X_Position <= 0)
            {
                player2.Score++;
                ResetBall();
            }
            else if (ball.X_Position >= Form1.ActiveForm.ClientSize.Width - ball.Size)
            {
                player1.Score++;
                ResetBall();
            }
        }

        public void DetectPaddleCollision(Paddle paddle, ref int xSpeed, int direction)
        {
            // Ball's center coordinates for collision detection
            int ballCenterX = ball.X_Position + ball.Size / 2;
            int ballCenterY = ball.Y_Position + ball.Size / 2;

            // Check if the ball is within the horizontal range of the paddle
            bool ballWithinPaddleHorizontalRange =
                (direction < 0 && ballCenterX <= paddle.X_Position + paddle.Width && ball.X_Speed < 0) ||  // Moving toward Player 1's paddle
                (direction > 0 && ballCenterX >= paddle.X_Position && ball.X_Speed > 0); // Moving toward Player 2's paddle

            if (!ballWithinPaddleHorizontalRange)
                return; // No horizontal collision


            // Ball is in vertical range of the paddle
            if (ballCenterY >= paddle.Y_Position && ballCenterY <= paddle.Y_Position + paddle.Height)
            {
                // Increment the collision count
                paddleCollisionCount++;

                // Ball hits the side of the paddle (reverse X-speed)
                xSpeed = -xSpeed;

                // Move the ball horizontally away from the paddle to avoid getting stuck inside
                if (direction < 0) // Moving left, Player 1's paddle
                {
                    ball.X_Position = paddle.X_Position + paddle.Width + 1; // Move ball to the right after hitting side
                }
                else // Moving right, Player 2's paddle
                {
                    ball.X_Position = paddle.X_Position - ball.Size - 1; // Move ball to the left after hitting side
                }
            }
        }

        public void ResetBall()
        {
            // Reset ball to the center
            ball.X_Position = Form1.ActiveForm.ClientSize.Width / 2;
            ball.Y_Position = Form1.ActiveForm.ClientSize.Height / 2;

            // Reset collision count
            paddleCollisionCount = 0;

            // Ball X and Y direction movement randomized 
            // -(3-5) < X/Y < (3-5)
            Random random = new();
            ball.X_Speed = random.Next(3, 6) * (random.Next(0, 2) == 0 ? -1 : 1);
            ball.Y_Speed = random.Next(3, 6) * (random.Next(0, 2) == 0 ? -1 : 1);
        }

        public void Draw(Graphics g)
        {
            ball.Draw(g);
            player1.Paddle.Draw(g);
            player2.Paddle.Draw(g);
        }
    }
}
