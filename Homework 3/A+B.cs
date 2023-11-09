using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

public class SystemAttackChartsForm : Form
{
    private NumericUpDown numM;
    private NumericUpDown numN;
    private NumericUpDown numP;
    private NumericUpDown numNthAttack;
    private Chart chart1;
    private Chart chart2;
    private Chart chart3;
    private Chart chart4;

    public SystemAttackChartsForm()
    {
        Text = "System Attack Charts";
        Size = new Size(1000, 600);

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Label labelM = new Label
        {
            Text = "Number of Systems (M):",
            Location = new Point(10, 10)
        };

        numM = new NumericUpDown
        {
            Minimum = 1,
            Value = 10,
            Location = new Point(200, 10)
        };

        Label labelN = new Label
        {
            Text = "Number of Attacks (N):",
            Location = new Point(10, 40)
        };

        numN = new NumericUpDown
        {
            Minimum = 1,
            Value = 10,
            Location = new Point(200, 40)
        };

        Label labelP = new Label
        {
            Text = "Probability of successful attack (P):",
            Location = new Point(10, 70)
        };

        numP = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 1,
            DecimalPlaces = 1,
            Increment = 0.1M,
            Value = 0.3M,
            Location = new Point(250, 70)
        };

        Label labelNthAttack = new Label
        {
            Text = "N-th attack:",
            Location = new Point(10, 100)
        };

        numNthAttack = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 100000,
            Value = 2,
            Location = new Point(150, 100)
        };

        chart1 = CreateChart("System Attacks");
        chart2 = CreateChart("Frequency");
        chart3 = CreateChart("Relative Frequency");
        chart4 = CreateChart("Normalized Ratio");

        FlowLayoutPanel chartPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Location = new Point(300, 10),
            Size = new Size(700, 500)
        };

        chartPanel.Controls.Add(chart1);
        chartPanel.Controls.Add(chart2);
        chartPanel.Controls.Add(chart3);
        chartPanel.Controls.Add(chart4);

        Controls.Add(labelM);
        Controls.Add(numM);
        Controls.Add(labelN);
        Controls.Add(numN);
        Controls.Add(labelP);
        Controls.Add(numP);
        Controls.Add(labelNthAttack);
        Controls.Add(numNthAttack);
        Controls.Add(chartPanel);

        numM.ValueChanged += (sender, e) => UpdateCharts();
        numN.ValueChanged += (sender, e) => UpdateCharts();
        numP.ValueChanged += (sender, e) => UpdateCharts();
        numNthAttack.ValueChanged += (sender, e) => CalculateFrequencyHistogram();
    }

    private Chart CreateChart(string title)
    {
        Chart chart = new Chart
        {
            Size = new Size(700, 120),
            Area3DStyle = Area3DStyle.None,
            Titles = { new Title(title, Docking.Top, new Font("Arial", 12)) }
        };

        ChartArea chartArea = new ChartArea();
        chart.ChartAreas.Add(chartArea);

        Series series = new Series
        {
            ChartType = SeriesChartType.Line,
            BorderWidth = 2,
            MarkerStyle = MarkerStyle.None,
        };

        chart.Series.Add(series);
        return chart;
    }

    private void UpdateCharts()
    {
        int M = (int)numM.Value;
        int N = (int)numN.Value;
        double p = (double)numP.Value;
        int nthAttack = (int)numNthAttack.Value;

        var systemAttackData = GenerateAttacks(N, p, M);
        var frequencyData = CalculateFrequency(systemAttackData, N, p, M);

        UpdateChart(chart1, "System Attacks", systemAttackData, N);
        UpdateChart(chart2, "Frequency", frequencyData.Item1, N);
        UpdateChart(chart3, "Relative Frequency", frequencyData.Item2, N);
        UpdateChart(chart4, "Normalized Ratio", frequencyData.Item3, N);

        CalculateFrequencyHistogram();
    }

    private void UpdateChart(Chart chart, string title, double[][] data, int N)
    {
        chart.Titles[0].Text = title;

        chart.Series[0].Points.Clear();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                chart.Series[0].Points.AddY(data[i][j]);
            }
        }
    }

    private double[][] GenerateAttacks(int N, double p, int M)
    {
        Random random = new Random();
        double[][] systemAttacks = new double[M][];

        for (int i = 0; i < M; i++)
        {
            systemAttacks[i] = new double[N];
            int sumAttacks = 0;

            for (int j = 0; j < N; j++)
            {
                if (random.NextDouble() >= p)
                {
                    sumAttacks += 1;
                }
                else
                {
                    sumAttacks -= 1;
                }
                systemAttacks[i][j] = sumAttacks;
            }
        }

        return systemAttacks;
    }

    private Tuple<double[][], double[][], double[][]> CalculateFrequency(double[][] systemAttacks, int N, double p, int M)
    {
        double[][] cumulativeFrequencies = new double[M][];
        double[][] relativeFrequencies = new double[M][];
        double[][] normalizedRatios = new double[M][];

        for (int i = 0; i < M; i++)
        {
            cumulativeFrequencies[i] = new double[N];
            relativeFrequencies[i] = new double[N];
            normalizedRatios[i] = new double[N];

            int cumulativeSum = 0;
            int totalAttacks = 0;

            for (int j = 0; j < N; j++)
            {
                totalAttacks++;

                if (systemAttacks[i][j] >= nthAttack)
                {
                    cumulativeSum++;
                }

                cumulativeFrequencies[i][j] = cumulativeSum;
                relativeFrequencies[i][j] = (double)cumulativeSum / totalAttacks;
                normalizedRatios[i][j] = Math.Sqrt(totalAttacks) > 0 ? (double)cumulativeSum / Math.Sqrt(totalAttacks) : 0;
            }
        }

        return Tuple.Create(cumulativeFrequencies, relativeFrequencies, normalizedRatios);
    }

    private void CalculateFrequencyHistogram()
    {
        int M = (int)numM.Value;
        int N = (int)numN.Value;
        double p = (double)numP.Value;

        double[][] systemAttacks = GenerateAttacks(N, p, M);
        var frequencyData = CalculateFrequency(systemAttacks, N, p, M);

        int[] lastAttacks = new int[M];
        int[] nthAttacks = new int[M];

        for (int i = 0; i < M; i++)
        {
            lastAttacks[i] = (int)systemAttacks[i][N - 1];
            nthAttacks[i] = (int)systemAttacks[i][nthAttack - 1];
        }

        int max = Math.Max(Math.Max(lastAttacks.Max(), nthAttacks.Max()), 0);
        int min = Math.Min(Math.Min(lastAttacks.Min(), nthAttacks.Min()), 0);

        int[] yAxes = new int[max - min + 1];
        int[] lastAttackFrequency = new int[yAxes.Length];
        int[] nthAttackFrequency = new int[yAxes.Length];

        for (int i = 0; i < yAxes.Length; i++)
        {
            yAxes[i] = max - i;
            lastAttackFrequency[i] = lastAttacks.Count(x => x == yAxes[i]);
            nthAttackFrequency[i] = nthAttacks.Count(x => x == yAxes[i]);
        }

        chart1.Series[0].Points.Clear();
        chart2.Series[0].Points.Clear();

        for (int i = 0; i < yAxes.Length; i++)
        {
            chart1.Series[0].Points.AddXY(lastAttackFrequency[i], yAxes[i]);
            chart2.Series[0].Points.AddXY(nthAttackFrequency[i], yAxes[i]);
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new SystemAttackChartsForm());
    }
}
