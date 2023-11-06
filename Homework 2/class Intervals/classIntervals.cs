int randomVariateCount = 0;
int classIntervalCount = 0;

Console.Write("Random Variate Count: ");
string _rv = Console.ReadLine() ?? "0";
Console.Write("Class Interval Count: ");
string _ci = Console.ReadLine() ?? "0";

int.TryParse(_rv, out randomVariateCount);
int.TryParse(_ci, out classIntervalCount);


int[] counts = new int[classIntervalCount];
for (int i = 0; i < counts.Length; i++)
{
    counts[i] = 0;
}

Random random = new Random();
for (int i = 0; i < randomVariateCount; i++)
{
    double rv = random.NextDouble();
    int ci = (int)Math.Floor(rv * classIntervalCount);
    counts[ci]++;
}
Console.WriteLine("x\ty");
for (int i = 0; i < counts.Length; i++)
{
    Console.WriteLine($"{i}\t{counts[i]}");
}
