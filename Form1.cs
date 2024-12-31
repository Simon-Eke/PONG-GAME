using System.Reflection.Metadata;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Ball ball;
        private Paddle player1Paddle;
        private Paddle player2Paddle;
        private System.Windows.Forms.Timer gameTimer;

        private int paddleCollisionCount = 0;

        private int player1Score = 0;
        private int player2Score = 0;

        private bool player1Up = false;
        private bool player1Down = false;
        private bool player2Up = false;
        private bool player2Down = false;


        public Form1()
        {
            InitializeComponent();
            InitializeGame();

            this.DoubleBuffered = true;
        }

        private void InitializeGame()
        {
            ball = new Ball(250, 200, 4, 4);
            player1Paddle = new Paddle(20, 150, 10, 100);
            player2Paddle = new Paddle(780, 150, 10, 100);

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            ball.Move();
            DetectCollisions();
            UpdatePlayerMovement(); // Continuously check player inputs

            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Player 1 controls
            if (e.KeyCode == Keys.W) player1Up = true; // Move up
            if (e.KeyCode == Keys.S) player1Down = true; // Move down

            // Player 2 controls
            if (e.KeyCode == Keys.Up) player2Up = true; // Move up
            if (e.KeyCode == Keys.Down) player2Down = true; // Move down
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            // Player 1 controls
            if (e.KeyCode == Keys.W) player1Up = false; // Stop moving up
            if (e.KeyCode == Keys.S) player1Down = false; // Stop moving down

            // Player 2 controls
            if (e.KeyCode == Keys.Up) player2Up = false; // Stop moving up
            if (e.KeyCode == Keys.Down) player2Down = false; // Stop moving down
        }

        private void UpdatePlayerMovement()
        {
            if (player1Up) player1Paddle.Move(-5); // Move up
            if (player1Down) player1Paddle.Move(5); // Move down

            if (player2Up) player2Paddle.Move(-5); // Move up
            if (player2Down) player2Paddle.Move(5); // Move down
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Set the background color to black
            g.Clear(Color.Black);

            ball.Draw(g);
            player1Paddle.Draw(g);
            player2Paddle.Draw(g);

            // Display the score
            g.DrawString($"Player 1: {player1Score}  Player 2: {player2Score}  Speed: {Math.Abs(ball.X_Speed)}",
                         new Font("Arial", 16), Brushes.White, 10, 10);
        }

        private void DetectCollisions()
        {
            // Ball colliding with top or bottom wall
            if (ball.Y_Position <= 0 || ball.Y_Position >= this.ClientSize.Height - ball.Size)
            {
                ball.Y_Speed = -ball.Y_Speed; // Reverse vertical direction
            }

            // Ball colliding with Paddles (for both players)
            if (ball.X_Speed < 0) // Moving toward player 1's paddle
            {
                DetectPaddleCollision(player1Paddle, ref ball.X_Speed, -1);
            }
            else if (ball.X_Speed > 0) // Moving toward player 2's paddle
            {
                DetectPaddleCollision(player2Paddle, ref ball.X_Speed, 1);
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
                player2Score++;
                ResetBall();
            }
            else if (ball.X_Position >= this.ClientSize.Width - ball.Size)
            {
                player1Score++;
                ResetBall();
            }
        }

        private void DetectPaddleCollision(Paddle paddle, ref int xSpeed, int direction)
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

        private void ResetBall()
        {
            // Reset ball to the center
            ball.X_Position = this.ClientSize.Width / 2;
            ball.Y_Position = this.ClientSize.Height / 2;

            // Reset collision count
            paddleCollisionCount = 0; 

            // Ball X and Y direction movement
            Random random = new();
            int randomXSpeed = random.Next(3, 6);
            int randomYSpeed = random.Next(3, 6);

            ball.X_Speed = random.Next(0, 2) == 0 ? -randomXSpeed : randomXSpeed;
            ball.Y_Speed = random.Next(0, 2) == 0 ? -randomYSpeed : randomYSpeed;
        }
    }
}
