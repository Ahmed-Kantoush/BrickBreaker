using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBreaker
{
    public partial class Form1 : Form
    {
        int score = 0;
        bool inGame = false;
        Point a;
        Point b;
        public Form1()
        {
            InitializeComponent();
            Gameover.Hide();
            Win.Hide();
            a = Ball.Location;
            b = Paddle.Location;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && (Gameover.Visible || Win.Visible))
            {
                start.Show();
                inGame = false;
                Cursor.Show();
                score = 0;
                Win.Hide();
                Gameover.Hide();
                Score.Text = "Score: 0";
                Ball.Location = a;
                Paddle.Location = b;
                foreach (Control c in this.Controls)
                {
                    if (c is PictureBox && (string)c.Tag == "removed")
                    {
                        c.Visible = true;
                        c.Tag = "brick";
                    }
                }
            }
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!inGame)
            {
                double x = this.PointToClient(Cursor.Position).X - Ball.Left - 10;
                double y = Ball.Top - this.PointToClient(Cursor.Position).Y + 10;
                if (y <= 10)
                    ball.angle = 45;
                else
                    ball.angle = Math.Atan(y / x) * 180 / Math.PI;

                ball.speed_x = (int)(ball.speed * Math.Cos(ball.angle * Math.PI / 180));
                ball.speed_y = (int)(ball.speed * Math.Sin(ball.angle * Math.PI / 180)) * -1;
                Cursor.Hide();
                start.Hide();
                timer1.Enabled = true;
                inGame = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            PaddleCollision();
            Collision();
            Ball.Left += ball.speed_x;
            Ball.Top += ball.speed_y;
            PaddleMovement();
            gameover();
        }

        public void PaddleCollision()
        {
            if (Paddle.Bounds.IntersectsWith(Ball.Bounds))
            {
                if (Paddle.Left - Ball.Left >= 0)
                {
                    ball.angle = (Paddle.Left - Ball.Left) * 5.3;
                    ball.speed_x = (int) (ball.direction * ball.speed * Math.Sin(ball.angle * Math.PI / 180));
                    ball.speed_x *= (ball.direction > 0) ? -1 : 1;
                    ball.speed_y = (int) (-1 * ball.speed * Math.Cos(ball.angle * Math.PI / 180));
                }
                else if (Ball.Left - Paddle.Left >= 60)
                {
                    ball.angle = (Ball.Left - Paddle.Left - 60) * 5.3;
                    ball.speed_x = (int)(ball.direction * ball.speed * Math.Sin(ball.angle * Math.PI / 180));
                    ball.speed_x *= (ball.direction > 0) ? 1 : -1;
                    ball.speed_y = (int)(-1 * ball.speed * Math.Cos(ball.angle * Math.PI / 180));
                }
                else
                {
                    ball.angle = 45;
                    ball.speed_x = (int)(ball.direction * ball.speed * Math.Sin(ball.angle * Math.PI / 180));
                    ball.speed_y = (int)(-1 * ball.speed * Math.Cos(ball.angle * Math.PI / 180));
                }
            }
        }

        public void Collision()
        {
            if (Ball.Left <= 0 || Ball.Left >= ClientRectangle.Width - ball.size)
                ball.speed_x *= -1;

            if (Ball.Top <= 0)
                ball.speed_y *= -1;

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && (string)c.Tag == "brick")
                    if (c.Bounds.IntersectsWith(Ball.Bounds))
                    {
                        c.Tag = "removed";
                        c.Hide();
                        score++;
                        Score.Text = "Score: " + score;
                        ball.speed_y *= -1;
                    }
            }
        }

        private void PaddleMovement()
        {
            int x = this.PointToClient(Cursor.Position).X - 37;
            if (x >= 0 && x <= ClientRectangle.Width - 75)
                Paddle.Left = x;
            else if (x <= 0)
                Paddle.Left = 0;
            else
                Paddle.Left = ClientRectangle.Width - 75;
        }

        public void gameover()
        {
            if (Ball.Top > ClientRectangle.Height)
            {
                Gameover.Visible = true;
                timer1.Enabled = false;
            }

            if (score == 24)
            {
                timer1.Enabled = false;
                Win.Show();
            }
        }
    }


    public class ball
    {
        public static int size = 15;
        public static int speed = 10;
        public static double angle = 45;
        public static int direction;
        private static int x;
        public static int speed_x
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                if (value > 0)
                    direction = 1;
                else
                    direction = -1;
            }
        }
        public static int speed_y;
    }
}
