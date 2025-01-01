using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Game
    {
        private Random random;

        private Ball ball;
        private Player player1;
        private Player player2;

        private int paddleCollisionCount;
        private int nextCollisionThreshold; // The number of collisions until the speed increases

        public Game()
        {
            random = new Random(); // Initialize the random generator for later use

            ball = new Ball(400, random.Next(1, 450), 4 * (random.Next(0, 2) == 0 ? -1 : 1), 4 * (random.Next(0, 2) == 0 ? -1 : 1));
            player1 = new Player(20);
            player2 = new Player(780);

            paddleCollisionCount = 0; // Initialize collision count
            nextCollisionThreshold = random.Next(3, 7); // Randomly determine when the next speed increase should occur (between 3 and 6 collisions)
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

            // Check if it's time to increase the speed of the ball.
            if (paddleCollisionCount >= nextCollisionThreshold)
            {
                // Time to increase the ball's speed
                // If the ball is moving left (negative X_Speed), decrease its speed (make it more negative)
                // If the ball is moving right (positive X_Speed), increase its speed (make it more positive)
                ball.X_Speed = ball.X_Speed < 0 ? ball.X_Speed - 1 : ball.X_Speed + 1;

                paddleCollisionCount = 0; // Reset the collision count after the speed increase

                // Randomize the number of collisions needed for the next speed increase (between 3 and 6)
                nextCollisionThreshold = random.Next(3, 7);
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
            // Reset ball to the center line but in a slight different height.
            ball.X_Position = Form1.ActiveForm.ClientSize.Width / 2;
            ball.Y_Position = random.Next(0, Form1.ActiveForm.ClientSize.Height);

            // Reset collision count
            paddleCollisionCount = 0;

            // Ball X and Y direction movement randomized 
            // -(3-5) < Y < (3-5) && -(4-5) < X < (4-5)
            ball.X_Speed = random.Next(4, 6) * (random.Next(0, 2) == 0 ? -1 : 1);
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
