using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Dictionary<string, string> knowledgeBase;

    static void Main()
    {
        Console.WriteLine("Hello, this is a C# console application!");

        knowledgeBase = new Dictionary<string, string>
        {
            { "What is your name?", "My name is Ben." },
            { "How does the Dice Coefficient work?", "The Dice Coefficient measures the similarity between two strings." },
        };

        while (true)
        {
            Console.Write("Ask a question (or type 'exit' to end): ");
            string userQuestion = Console.ReadLine();

            if (userQuestion.ToLower() == "exit")
            {
                break;
            }

            string answer = GetAnswer(userQuestion);
            Console.WriteLine("Answer: " + answer);
            Console.WriteLine();
            continue;
        }

        Console.WriteLine("Goodbye!");
    }

    static string GetAnswer(string userQuestion)
    {
        double maxSimilarity = 0.0;
        string bestMatch = "";

        foreach (var questionInKnowledgeBase in knowledgeBase.Keys)
        {
            double similarity = CalculateCustomSimilarity(userQuestion, questionInKnowledgeBase);
            Console.WriteLine($"Accuracy is {similarity} : {questionInKnowledgeBase}");
            if (similarity > maxSimilarity)
            {
                maxSimilarity = similarity;
                bestMatch = knowledgeBase[questionInKnowledgeBase];
            }
        }

        if (maxSimilarity > 0.5)
        {
            return bestMatch;
        }

        return "Sorry, No data found related to this question.";
    }

    static double CalculateCustomSimilarity(string str1, string str2)
    {
        HashSet<string> set1 = new HashSet<string>(str1.Split());
        HashSet<string> set2 = new HashSet<string>(str2.Split());

        // Calculate similarity for all possible subsequences
        double maxSimilarity = 0.0;
        foreach (var subsequence in GetAllSubsequences(set1))
        {
            double similarity = CalculateSimilarity(subsequence, set2);
            maxSimilarity = Math.Max(maxSimilarity, similarity);
        }

        return maxSimilarity;
    }

    static double CalculateSimilarity(HashSet<string> set1, HashSet<string> set2)
    {
        int intersection = 2 * (new HashSet<string>(set1).Intersect(set2)).Count();
        int union = set1.Count + set2.Count;

        return intersection / (double)union;
    }

    static IEnumerable<HashSet<string>> GetAllSubsequences(HashSet<string> set)
{
    var powerSet = new HashSet<HashSet<string>> { new HashSet<string>() };

    foreach (var element in set)
    {
        var newSubsets = new List<HashSet<string>>();

        foreach (var subset in powerSet)
        {
            var newSubset = new HashSet<string>(subset) { element };
            newSubsets.Add(newSubset);
        }

        powerSet.UnionWith(newSubsets);
    }

    return powerSet;
}
}
