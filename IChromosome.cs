namespace GeneticAlgorithmTR;

interface IChromosome : ICloneable
{
    const int Capacity = 22;
    const double Step = 3.0 / (1 << 22);

    double DoubleValue { get; }
    int this[int i] { get; set; }
}

static class ChromosomeUtil
{
    public static double CalculateBinary(IEnumerable<int> encoding)
    {
        var index = 0;
        foreach (var i in encoding)
        {
            index = (index * 2) + i;
        }
        return (index * IChromosome.Step) - 1;
    }

    public static IEnumerable<int> UnGray(IEnumerable<int> encoding)
    {
        var ungrayCode = new int[encoding.Count()];
        ungrayCode[0] = encoding.First();
        for (var i = 1; i < encoding.Count(); i++)
        {
            ungrayCode[i] = (ungrayCode[i - 1] + encoding.ElementAt(i)) % 2;
        }
        return ungrayCode;
    }

    public static double CalculateGray(IEnumerable<int> encoding)
    {
        var digit = encoding.First();
        var index = digit;
        for (var i = 1; i < encoding.Count(); i++)
        {
            digit = (digit + encoding.ElementAt(i)) % 2;
            index = (index * 2) + digit;
        }
        return (index * IChromosome.Step) - 1;
    }
}