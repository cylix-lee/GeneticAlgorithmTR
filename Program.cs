using System.Text;
using GeneticAlgorithmTR.Chromosome;

namespace GeneticAlgorithmTR;

class Program
{
    const int PopulationSize = 512;
    const double CrossOverProbability = 0.6;
    const double MutationProbability = 0.05;
    const int EvolutionRound = 1000;

    static void Main()
    {
        var population = new Population<GrayEncodingChromosome>(PopulationSize, CrossOverProbability, MutationProbability);
        for (var i = 0; i < EvolutionRound; i++)
        {
            if (i % 100 == 0)
                Log.Info($"Evolution round {i}...");
            population.Select();
            population.CrossOver();
            population.Mutate();
            population.CheckAndForceMutate();
        }
        population.Select();

        var result = population.StopEvolution();
        Log.Ok($"Final result: {result.First().Fitness}");
    }
}

static class EnumerableCollectAsStringExtension
{
    public static string CollectAsString<T>(this IEnumerable<T> enumerable) where T : notnull
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in enumerable)
        {
            stringBuilder.Append($"{item.ToString()} ");
        }
        return stringBuilder.ToString();
    }
}