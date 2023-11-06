using System;
using System.Collections.Generic;

public class DataAnalyzer
{
    public Dictionary<string, int> ComputeFrequencies(List<double> data, int totalDataPoints, bool isContinuous = false, double classInterval = 1)
    {
        Dictionary<string, int> absoluteFrequency = new Dictionary<string, int>();

        if (isContinuous)
        {
            foreach (var item in data)
            {
                var interval = (Math.Floor(item / classInterval) * classInterval).ToString("F2");
                if (absoluteFrequency.ContainsKey(interval))
                {
                    absoluteFrequency[interval]++;
                }
                else
                {
                    absoluteFrequency[interval] = 1;
                }
            }
        }
        else
        {
            foreach (var item in data)
            {
                if (absoluteFrequency.ContainsKey(item.ToString()))
                {
                    absoluteFrequency[item.ToString()]++;
                }
                else
                {
                    absoluteFrequency[item.ToString()] = 1;
                }
            }
        }

        Dictionary<string, double> relativeFrequency = new Dictionary<string, double>();
        Dictionary<string, string> percentageFrequency = new Dictionary<string, string>();

        foreach (var category in absoluteFrequency)
        {
            relativeFrequency[category.Key] = (double)category.Value / totalDataPoints;
            percentageFrequency[category.Key] = (relativeFrequency[category.Key] * 100).ToString("F2") + "%";
        }

        return new Dictionary<string, int>(absoluteFrequency);
    }

    public (List<string>, List<int>, List<double>) ParseCSV(string content)
    {
        List<string> qualitativeData = new List<string>();
        List<int> discreteData = new List<int>();
        List<double> continuousData = new List<double>();

        var lines = content.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            var columns = lines[i].Split(',');
            var qualitative = columns[25].Trim();
            var discrete = int.Parse(columns[4].Trim());
            var continuous = double.Parse(columns[18].Trim());

            qualitativeData.Add(qualitative);
            discreteData.Add(discrete);
            continuousData.Add(continuous);
        }

        return (qualitativeData, discreteData, continuousData);
    }
}

public class Program
{
    public static void Main()
    {
        string content = "ProfessionalLife.csv";
        DataAnalyzer dataAnalyzer = new DataAnalyzer();

        var (qualitativeData, discreteData, continuousData) = dataAnalyzer.ParseCSV(content);

        var totalDataPoints = qualitativeData.Count + discreteData.Count + continuousData.Count;

        var qualitativeFrequency = dataAnalyzer.ComputeFrequencies(qualitativeData, totalDataPoints);
        var discreteFrequency = dataAnalyzer.ComputeFrequencies(discreteData, totalDataPoints);
        var classInterval = 0.01;
        var continuousFrequency = dataAnalyzer.ComputeFrequencies(continuousData, totalDataPoints, true, classInterval);

        // Joint Frequency where k = 2 for two variables
        var jointFrequency = ComputeJointFrequency(new List<List<string>> { qualitativeData, discreteData });

        DisplayTable("Qualitative Variable", "Dream Works", qualitativeFrequency);
        DisplayTable("Discrete Variable", "Hard Worker", discreteFrequency);
        DisplayTable("Continuous Variable", "Height", continuousFrequency);
        DisplayTable("Joint Frequency", "Joint Frequency", jointFrequency);
    }

    public static Dictionary<string, int> ComputeJointFrequency(List<List<string>> dataArrays)
    {
        Dictionary<string, int> jointFrequency = new Dictionary<string, int>();

        for (int i = 0; i < dataArrays[0].Count; i++)
        {
            string key = string.Join(" & ", dataArrays.ConvertAll(array => array[i]));
            if (jointFrequency.ContainsKey(key))
            {
                jointFrequency[key]++;
            }
            else
            {
                jointFrequency[key] = 1;
            }
        }

        var totalDataPoints = jointFrequency.Values.Sum();

        Dictionary<string, double> relativeFrequency = new Dictionary<string, double>();
        Dictionary<string, string> percentageFrequency = new Dictionary<string, string>();

        foreach (var item in jointFrequency)
        {
            relativeFrequency[item.Key] = (double)item.Value / totalDataPoints;
            percentageFrequency[item.Key] = (relativeFrequency[item.Key] * 100).ToString("F2") + "%";
        }

        return jointFrequency;
    }

    public static void DisplayTable(string title, string subTitle, Dictionary<string, int> frequencies)
    {
        Console.WriteLine($"Title: {title}");
        Console.WriteLine($"Subtitle: {subTitle}");
        Console.WriteLine("Category\tAbsolute");

        foreach (var item in frequencies)
        {
            Console.WriteLine($"{item.Key}\t\t{item.Value}");
        }

        Console.WriteLine();
    }
}
