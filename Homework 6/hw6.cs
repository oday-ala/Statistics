using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SecuritySimulationApp
{
    public partial class MainForm : Form
    {
        private int N = 50; // Default value for N
        private int M = 100; // Default value for M
        private double P = 0.5; // Default value for P
        private int S = 60; // Default value for S
        private List<int> PValues = new List<int> { 60 }; 
        private const int ScaleFactor = 2; 

        public MainForm()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.SuspendLayout();
            this.chartPictureBox = new System.Windows.Forms.PictureBox();
            this.numAttacksNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.numSystemsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.penetrationRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.securityThresholdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.updateChartButton = new System.Windows.Forms.Button();

            this.updateChartButton.Click += new System.EventHandler(this.updateChartButton_Click);

            this.ResumeLayout(false);
        }

        private void InitializeComponent()
        {

            this.Controls.Add(this.chartPictureBox);
            this.Controls.Add(this.numAttacksNumericUpDown);
            this.Controls.Add(this.numSystemsNumericUpDown);
            this.Controls.Add(this.penetrationRateNumericUpDown);
            this.Controls.Add(this.securityThresholdNumericUpDown);
            this.Controls.Add(this.updateChartButton);
        }

        private void UpdateChart()
        {
            N = (int)numAttacksNumericUpDown.Value;
            M = (int)numSystemsNumericUpDown.Value;
            P = (double)penetrationRateNumericUpDown.Value;
            S = (int)securityThresholdNumericUpDown.Value;

            DrawChart();
        }

        private void DrawChart()
        {
            Bitmap chartBitmap = new Bitmap(chartPictureBox.Width, chartPictureBox.Height);
            using (Graphics g = Graphics.FromImage(chartBitmap))
            {
                g.Clear(Color.White);

                // Draw chart here (placeholder)
                DrawAxis(g);
                DrawSimulation(g);
                DrawHistogram(g);
            }

            chartPictureBox.Image = chartBitmap;
        }

        private void DrawAxis(Graphics g)
        {
            // Draw axis logic (placeholder)
            g.DrawLine(Pens.Black, 50, 0, 50, chartPictureBox.Height);
            g.DrawLine(Pens.Black, 50, chartPictureBox.Height / 2, chartPictureBox.Width, chartPictureBox.Height / 2);

            for (int i = -N; i <= N; i += 2)
            {
                g.DrawLine(Pens.Gray, 50, chartPictureBox.Height / 2 + i * 10, chartPictureBox.Width, chartPictureBox.Height / 2 + i * 10);
            }
        }

        private void DrawSimulation(Graphics g)
        {
            // Draw simulation paths logic (placeholder)
            Random random = new Random();

            for (int system = 0; system < M; system++)
            {
                List<int> scores = new List<int>();
                int score = 0;

                for (int attack = 0; attack < N; attack++)
                {
                    double probability = random.NextDouble();
                    score += probability < P ? 1 : -1;
                    scores.Add(score);
                }

                DrawPath(g, scores, system);
            }
        }

        private void DrawPath(Graphics g, List<int> scores, int system)
        {
            // Draw simulation path logic (placeholder)
            List<Color> colors = GenerateColors(M);

            using (Pen pen = new Pen(colors[system]))
            {
                Point[] points = new Point[N];

                for (int attack = 0; attack < N; attack++)
                {
                    int x = 50 + (attack + 1) * 10;
                    int y = chartPictureBox.Height / 2 - scores[attack] * 10;
                    points[attack] = new Point(x, y);
                }

                g.DrawLines(pen, points);
            }
        }

        private void DrawHistogram(Graphics g)
        {
            // Draw histogram logic (placeholder)
            for (int threshold of PValues)
            {
                Dictionary<int, int> counts = CountFirstThresholds(threshold);

                int maxBarWidth = N * 10;

                foreach (var entry in counts)
                {
                    int count = entry.Value;
                    int barLength = (count / M) * maxBarWidth;
                    int barWidth = 10;
                    int barStartY = entry.Key < 0 ? chartPictureBox.Height / 2 - entry.Key * 10 : chartPictureBox.Height / 2 - entry.Key * 10 - 22;
                    int barStartX = 50 + N * 10 - barLength;

                    using (Brush brush = new SolidBrush(entry.Key < 0 ? Color.FromArgb(0, 255, 0, 0.6) : Color.FromArgb(255, 0, 0, 0.6)))
                    {
                        g.FillRectangle(brush, barStartX, barStartY, barLength, barWidth);
                    }

                    g.DrawRectangle(Pens.Black, barStartX, barStartY, barLength, barWidth);
                }
            }
        }

        private Dictionary<int, int> CountFirstThresholds(int threshold)
        {
            Dictionary<int, int> counts = new Dictionary<int, int>();

            for (int system = 0; system < M; system++)
            {
                int score = 0;
                bool hasReachedS = false;

                for (int attack = 0; attack < N; attack++)
                {
                    score += Math.random() < P ? 1 : -1;

                    if (!hasReachedS && score <= S)
                    {
                        counts[S] = counts.ContainsKey(S) ? counts[S] + 1 : 1;
                        hasReachedS = true;
                    }

                    if (score >= threshold)
                    {
                        counts[threshold] = counts.ContainsKey(threshold) ? counts[threshold] + 1 : 1;
                    }
                }
            }

            return counts;
        }

        private List<Color> GenerateColors(int numColors)
        {
            List<Color> colors = new List<Color>();
            Random random = new Random();

            for (int i = 0; i < numColors; i++)
            {
                Color color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                colors.Add(color);
            }

            return colors;
        }

        private void updateChartButton_Click(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private System.Windows.Forms.PictureBox chartPictureBox;
        private System.Windows.Forms.NumericUpDown numAttacksNumericUpDown;
        private System.Windows.Forms.NumericUpDown numSystemsNumericUpDown;
        private System.Windows.Forms.NumericUpDown penetrationRateNumericUpDown;
        private System.Windows.Forms.NumericUpDown securityThresholdNumericUpDown;
        private System.Windows.Forms.Button updateChartButton;

        static class Program
        {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
