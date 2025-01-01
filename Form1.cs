using System.Reflection.Metadata;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Game game;
        private System.Windows.Forms.Timer gameTimer;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Focus();

            game = new Game();

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            game.Update();
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.W) game.GetPlayer1().MoveUp = true;
            if (e.KeyCode == Keys.S) game.GetPlayer1().MoveDown = true;
            if (e.KeyCode == Keys.Up) game.GetPlayer2().MoveUp = true;
            if (e.KeyCode == Keys.Down) game.GetPlayer2().MoveDown = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.W) game.GetPlayer1().MoveUp = false;
            if (e.KeyCode == Keys.S) game.GetPlayer1().MoveDown = false;
            if (e.KeyCode == Keys.Up) game.GetPlayer2().MoveUp = false;
            if (e.KeyCode == Keys.Down) game.GetPlayer2().MoveDown = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            game.Draw(g);
            // Use a font like "Courier New" or "Consolas" to get the blocky appearance.
            //g.DrawString($"Player 1: {game.GetPlayer1Score()} Player 2: {game.GetPlayer2Score()}  Speed: {Math.Abs(game.GetBallSpeed())}",
            //    new Font("Arial", 16), Brushes.White, 10, 10); - OLD CODE

            // Set up the blocky font (monospace)
            Font blockyFont = new Font("Consolas", 16, FontStyle.Bold);

            // Get the score as text
            string scoreText = $"Player 1: {game.GetPlayer1Score()}  Player 2: {game.GetPlayer2Score()}";

            // Calculate the size of the text to center it
            SizeF textSize = g.MeasureString(scoreText, blockyFont);

            // Define the rectangle for the box
            int boxWidth = (int)textSize.Width + 20; // Add some padding
            int boxHeight = (int)textSize.Height + 10; // Add padding for height
            int boxX = (Form1.ActiveForm.ClientSize.Width - boxWidth) / 2; // Center horizontally
            int boxY = 10; // Set some vertical margin from the top

            // Draw the box (border)
            g.DrawRectangle(Pens.White, boxX, boxY, boxWidth, boxHeight);

            // Draw the score text inside the box, centered
            g.DrawString(scoreText, blockyFont, Brushes.White, boxX + 10, boxY + 5); // Padding inside box
        }
    }
}
