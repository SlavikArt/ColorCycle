using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace ColorCycle
{
    public partial class Form1 : Form
    {
        private Timer timer1;
        private static int speedFactor = 5;

        private Color[] colors = new Color[]
        {
            Color.Black,
            Color.Red,
            Color.Yellow,
            Color.Green,
            Color.Blue,
            Color.Pink,
            Color.White
        };

        private Color targetColor;
        private Color currentColor;
        private int currentColorIndex = 0;

        private System.Timers.Timer timer2;
        private int clickCount = 0;
        private int maxClickCount = 0;

        public Form1()
        {
            InitializeComponent();

            targetColor = colors[currentColorIndex];
            currentColor = colors[currentColorIndex];

            timer1 = new Timer();
            timer1.Interval = 25;
            timer1.Tick += Timer1Handler;
            timer1.Start();

            timer2 = new System.Timers.Timer(1000);
            timer2.Elapsed += Timer2ElapsedHandler;
        }

        private void Timer1Handler(object sender, EventArgs e)
        {
            if (currentColor.ToArgb() == targetColor.ToArgb())
            {
                currentColorIndex = (currentColorIndex + 1) % colors.Length;
                targetColor = colors[currentColorIndex];
            }
            currentColor = GetNextColor(currentColor, targetColor);
            BackColor = currentColor;
        }

        private static Color GetNextColor(Color current, Color target)
        {
            int R = ApproachToColor(current.R, target.R);
            int G = ApproachToColor(current.G, target.G);
            int B = ApproachToColor(current.B, target.B);

            return Color.FromArgb(R, G, B);
        }

        private static int ApproachToColor(int current, int target)
        {
            if (current < target)
                return Math.Min(current + speedFactor, target);
            if (current > target)
                return Math.Max(current - speedFactor, target);
            return current;
        }

        private void Timer2ElapsedHandler(object sender, EventArgs e)
        {
            Invoke(UpdateInfo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!timer2.Enabled)
            {
                timer2.Start();
                label1.Text = "Осталось времени: 20 секунд";
            }

            clickCount++;
            button1.Text = $"Вы нажали: {clickCount} раз!";
        }

        private void UpdateInfo()
        {
            int timeLeft = int.Parse(label1.Text.Split(' ')[2]) - 1;
            label1.Text = $"Осталось времени: {timeLeft} секунд";

            if (timeLeft <= 0)
            {
                timer2.Stop();

                if (clickCount > maxClickCount)
                    maxClickCount = clickCount;

                MessageBox.Show($"Вы сделали {clickCount} кликов.\nМаксимальное количество кликов: {maxClickCount}");

                label2.Text = $"Максимальное количество кликов: {maxClickCount}";
                clickCount = 0;
                button1.Text = $"Нажми на меня что бы запустить таймер";
            }
        }
    }
}