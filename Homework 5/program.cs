using System;
using System.Diagnostics.Metrics;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Penetration_C_
{
    public partial class Form1 : Form
    {

        private bool isDragging = false;
        private Point lastCursorPosition;

        private bool isResizing = false;
        private Point resizeStart;
        private Size originalSize;

        PictureBox pictureBox1;

        Chart line_attacks, histogram_attacks, histogram_partial;
        Bitmap b_line, b_column, b_partial, b_final;


        public Form1()
        {
            InitializeComponent();

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = Int32.MaxValue;

            numericUpDown2.Minimum = 1;
            numericUpDown2.Maximum = Int32.MaxValue;

            numericUpDown3.Minimum = 1;
            numericUpDown3.Maximum = Int32.MaxValue;
        }

        int prev = 0;
        int curr = 1;
        System.Timers.Timer myTimer;
        bool status1 = false;

        int max_value;
        int max_histogram;
        int[] values_lines;

        ChartArea chartArea, histogramArea, histogramPartialArea;


        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1 = new PictureBox();
            pictureBox1.Location = new Point(0, 100);
            this.WindowState = FormWindowState.Maximized;
            pictureBox1.Height = this.Size.Height;
            pictureBox1.Width = this.Size.Width;
            //pictureBox1.BackColor = Color.Red;
            this.Controls.Add(pictureBox1);


            panel1.Width = pictureBox1.Width;
            panel1.Height = pictureBox1.Height - 139;
            panel1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int attack_ratio = (int)numericUpDown1.Value;
            int intervals = (int)numericUpDown2.Value;
            int number_servers = (int)numericUpDown3.Value;

            if (attack_ratio > intervals)
            {
                MessageBox.Show("Selected invalid values, exiting...");
                Environment.Exit(-1);
            }

            myTimer = new System.Timers.Timer();
            myTimer.Interval = 500;
            myTimer.Elapsed += myElapsed;
            myTimer.AutoReset = true;
            myTimer.Start();

            createAttackGraphs_Panel(attack_ratio, intervals, number_servers);

            myTimer.Stop();

        }



        private void myElapsed(object sender, ElapsedEventArgs e)
        {
            curr++;
        }

        private void myRefresh()
        {
            if (status1)
            {
                max_value = values_lines.Max();
                chartArea.AxisY.Maximum = max_value;
                histogramArea.AxisX.Maximum = max_value;
                histogramPartialArea.AxisX.Maximum = max_value;

                int temp;
                max_histogram = 0;
                for (int i = 0; i < values_lines.Length; i++)
                {
                    temp = values_lines.Count(s => s == values_lines[i]);
                    if (temp > max_histogram) { max_histogram = temp; }
                }
                histogramArea.AxisY.Maximum = max_histogram*1.05;

                this.line_attacks.DrawToBitmap(this.b_line, new Rectangle(0, 0, panel1.Width / 2, panel1.Height));
                this.histogram_partial.DrawToBitmap(this.b_partial, new Rectangle(panel1.Width / 4, 0, panel1.Width, panel1.Height));
                this.histogram_attacks.DrawToBitmap(this.b_column, new Rectangle(panel1.Width / 2, 0, panel1.Width, panel1.Height));

                this.b_line.MakeTransparent(Color.White);
                this.b_column.MakeTransparent(Color.White);
                this.b_partial.MakeTransparent(Color.White);

                using (Graphics g = Graphics.FromImage(this.b_final))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(this.b_line, new Rectangle(0, 0, panel1.Width, panel1.Height));
                    g.DrawImage(this.b_partial, new Rectangle(0, 0, panel1.Width, panel1.Height));
                    g.DrawImage(this.b_column, new Rectangle(0, 0, panel1.Width, panel1.Height));
                }

                this.panel1.BackgroundImage = this.b_final;
                this.panel1.BackgroundImageLayout = ImageLayout.Stretch;
                this.panel1.Refresh();
            }
            prev = curr;
        }



        private void createAttackGraphs_Panel(float attack_ratio, int intervals, int number_servers)
        {
            status1 = true;

            // Create the Chart for the security score 
            line_attacks = new Chart();
            line_attacks.Width = panel1.Width / 2;
            line_attacks.Height = panel1.Height;

            // Create chart for security scores
            chartArea = new ChartArea("SecurityChart");
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartArea.AxisY.LabelStyle.Format = "0";
            chartArea.Position = new ElementPosition(0, 0, 100, 100);
            line_attacks.ChartAreas.Add(chartArea);


            // Create chart for the histogram
            histogram_attacks = new Chart();
            histogram_attacks.Width = panel1.Width / 2;
            histogram_attacks.Height = panel1.Height;
            histogramArea = new ChartArea("HistogramChart");
            histogramArea.AxisY.Minimum = 0;
            histogramArea.AxisX.Minimum = 0;
            histogramArea.AxisX.MajorGrid.LineWidth = 0;
            histogramArea.AxisY.MajorGrid.LineWidth = 0;
            histogramArea.AxisX.LabelStyle.Format = "0";
            histogramArea.AxisX.LabelStyle.Enabled = false;
            histogramArea.AxisX.MajorTickMark.Enabled = false;
            histogramArea.AxisY.LabelStyle.Format = "0";
            histogramArea.Position = new ElementPosition(0, 1, 97, 99);
            histogram_attacks.ChartAreas.Add(histogramArea);

            // Create the Chart for the partial histogram
            histogram_partial = new Chart();
            histogram_partial.Width = panel1.Width;
            histogram_partial.Height = panel1.Height;
            histogramPartialArea = new ChartArea("HistogramChart");
            histogramPartialArea.AxisY.Minimum = 0;
            histogramPartialArea.AxisX.Minimum = 0;
            histogramPartialArea.AxisX.MajorGrid.LineWidth = 0;
            histogramPartialArea.AxisY.MajorGrid.LineWidth = 0;
            histogramPartialArea.AxisX.Enabled = AxisEnabled.False;
            histogramPartialArea.AxisY.Enabled = AxisEnabled.False;
            histogramPartialArea.Position = new ElementPosition(0, 2, 50, 97);
            histogram_partial.ChartAreas.Add(histogramPartialArea);


            // Create bitmap that will contain both charts
            b_line = new Bitmap(panel1.Width, panel1.Height);
            b_column = new Bitmap(panel1.Width, panel1.Height);
            b_partial = new Bitmap(panel1.Width, panel1.Height);
            b_final = new Bitmap(panel1.Width, panel1.Height);


            // Variables for the security score chart
            Series[] lines_array = new Series[number_servers];
            values_lines = new int[number_servers];
            Random rnd = new Random();


            // Initialization of security score chart
            for (int i = 0; i < lines_array.Length; i++)
            {
                Series line_series = new Series();
                lines_array[i] = line_series;
                line_series.Color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                line_series.ChartType = SeriesChartType.Line;
                line_attacks.Series.Add(line_series);
                line_series.Points.AddXY(0, 0);
                values_lines[i] = 0;
            }


            // Simulation to draw chart
            for (int i = 1; i < intervals + 1; i++)
            {

                if (i == intervals / 2)
                {
                    drawPartial(values_lines, number_servers);
                }

                for (int j = 0; j < number_servers; j++)
                {
                    Series series = lines_array[j];

                    if (rnd.NextDouble() >= (attack_ratio / intervals))
                    {
                        series.Points.AddXY(i, values_lines[j]);
                    }
                    else
                    {
                        series.Points.AddXY(i, values_lines[j] + 1);
                        values_lines[j] += 1;
                    }

                    line_attacks.Series.Append(series);
                    line_attacks.Update();
                }
                if (prev != curr) myRefresh();
            }
            myRefresh();



            Array.Sort(values_lines);
            int current = Int32.MinValue;
            for (int i = 0; i < values_lines.Length; i++)
            {
                if (values_lines[i] == current) continue;
                current = values_lines[i];
                Series histogram_series = new Series();
                histogram_series.ChartType = SeriesChartType.Bar;
                histogram_series.Color = Color.Red;
                histogram_series["PointWidth"] = "0.1";
                histogram_series.SmartLabelStyle.IsMarkerOverlappingAllowed = false;
                histogram_series.Points.Add(new DataPoint(current, values_lines.Count(s => s == current)));
                histogram_attacks.Series.Add(histogram_series);
                histogram_attacks.Update();
                if (prev != curr) myRefresh();
            }
            myRefresh();

            status1 = false;
        }



        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            this.panel1.BringToFront();
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursorPosition = e.Location;
            }

            if (e.Button == MouseButtons.Right)
            {
                isResizing = true;
                resizeStart = e.Location;
                originalSize = this.panel1.Size;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int deltaX = e.X - lastCursorPosition.X;
                int deltaY = e.Y - lastCursorPosition.Y;
                panel1.Left += deltaX;
                panel1.Top += deltaY;
            }

            if (isResizing)
            {
                int deltaX = e.X - resizeStart.X;
                int deltaY = e.Y - resizeStart.Y;

                // Calcola la nuova dimensione del pannello in base allo spostamento del mouse.
                int newWidth = originalSize.Width + deltaX;
                int newHeight = originalSize.Height + deltaY;

                if (newWidth > 0 && newHeight > 0)
                {
                    this.panel1.Size = new Size(newWidth, newHeight);
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isResizing = false;

        }

        private void drawPartial(int[] values_lines, int number_servers)
        {
            int[] temp = new int[number_servers];

            for (int i = 0; i < number_servers; i++) { temp[i] = values_lines[i]; }

            Array.Sort(temp);
            int current = Int32.MinValue;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == current) continue;
                current = temp[i];
                Series histogram_series = new Series();
                histogram_series.ChartType = SeriesChartType.Bar;
                histogram_series.Color = Color.Blue;
                histogram_series["PointWidth"] = "0.1";
                histogram_series.SmartLabelStyle.IsMarkerOverlappingAllowed = false;
                histogram_series.Points.Add(new DataPoint(current, temp.Count(s => s == current)));
                histogram_partial.Series.Add(histogram_series);
                histogram_partial.Update();
            }
        }
    }
}