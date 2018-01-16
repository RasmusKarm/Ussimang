using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeG
{
    
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;
            Settings.GameOver = false;
            Settings.Score = 0;

            //Ussi pea
            Snake.Clear();
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;
            Circle head = new Circle { X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos) };
            Snake.Add(head);

            Random rand = new Random();
            int rnd = rand.Next(0,4);

            switch (rnd)
            {
                case 0: Settings.direction = Direction.Up;
                    break;
                case 1:
                    Settings.direction = Direction.Down;
                    break;
                case 2:
                    Settings.direction = Direction.Left;
                    break;
                case 3:
                    Settings.direction = Direction.Right;
                    break;
            }

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }
        //Söök suvalisse kohta
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;
            food = new Circle { X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos) };
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            //Kui mäng on läbi siis oota enterit
            if (Settings.GameOver)
            {
                //Kui vajutatakse enterit siis läheb mäng uuesti käima
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                //värvi uss ära
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColor;
                    if (i == 0)
                        snakeColor = Brushes.Black;
                    else
                        snakeColor = Brushes.Black;

                    //Uss
                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                        Snake[i].Y * Settings.Height,
                                        Settings.Width + 14, Settings.Height + 14));


                    //Söök
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                food.Y * Settings.Height, Settings.Width + 16, Settings.Height + 16));

                }
            }
            else
            {
                string gameOver = "Game over \nYour final score is: " + Settings.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //Liiguta pead
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    //Maksimaalne mänguala laius ja kõrgus
                    int maxXPos = (pbCanvas.Size.Width / Settings.Width);
                    int maxYPos = (pbCanvas.Size.Height / Settings.Height);

                    //Kui uss puudutab seina siis mäng saab läbi
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    //Kui uss puudutab ise ennast siis ta sööb ennast(mäng läbi)
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                            Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //Kui uss satub sööda peale siis ta sööb selle ära
                    if ((Snake[0].X - food.X) < (pbCanvas.Size.Width * 0.003) && (Snake[0].X - food.X) > ((pbCanvas.Size.Width * 0.003) * -1) 
                        && (Snake[0].Y - food.Y) < (pbCanvas.Size.Height * 0.004) && (Snake[0].Y - food.Y) > ((pbCanvas.Size.Height * 0.004) * -1))
                    {
                        Eat();
                    }

                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            //Tee uss pikemaks
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //Lisa punkte
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Settings();

            //Seadistab mängukiiruse ja paneb taimer käima
            gameTimer.Interval = 12;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }
    }

    
}
