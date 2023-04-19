namespace GeneticAlgorithmTR;

class Individual<TChromosome> : ICloneable where TChromosome : IChromosome, new()
{
    public double Fitness { get; }
    public int this[int index]
    {
        get => chromosome[index];
        set => chromosome[index] = value;
    }

    readonly TChromosome chromosome;

    public Individual()
    {
        chromosome = new TChromosome();
        Fitness = (chromosome.DoubleValue * Math.Sin(10 * Math.PI * chromosome.DoubleValue)) + 2;
    }

    public Individual(Individual<TChromosome> another)
    {
        chromosome = (TChromosome)another.chromosome.Clone();
        Fitness = another.Fitness;
    }

    public object Clone() => new Individual<TChromosome>(this);
}