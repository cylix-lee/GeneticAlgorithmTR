namespace GeneticAlgorithmTR;

class Population<TChromosome> where TChromosome : IChromosome, new()
{
    readonly int populationSize;
    readonly double crossOverProbability;
    readonly double mutationProbability;
    List<Individual<TChromosome>> individuals;

    class Comparer : IComparer<Individual<TChromosome>>
    {
        public int Compare(Individual<TChromosome>? x, Individual<TChromosome>? y)
        {
            if (x == null || y == null) throw new NullReferenceException();

            var difference = y.Fitness - x.Fitness;
            if (difference < 0) return -1;
            else if (difference > 0) return 1;
            return 0;
        }
    }

    public Population(int populationSize, double crossOverProbability, double mutationProbability)
    {
        Log.Ok($"Population initialized with size={populationSize}, " +
            $"crossOverProbability={crossOverProbability}, " +
            $"mutationProbability={mutationProbability}");
        this.populationSize = populationSize;
        this.crossOverProbability = crossOverProbability;
        this.mutationProbability = mutationProbability;

        individuals = new(populationSize);
        for (var i = 0; i < populationSize; i++)
        {
            individuals.Add(new Individual<TChromosome>());
        }
    }

    public void Select()
    {
        var prefixSum = new double[individuals.Count + 1];
        prefixSum[0] = 0;
        for (var i = 0; i < individuals.Count; i++)
        {
            prefixSum[i + 1] = prefixSum[i] + individuals[i].Fitness;
        }

        var totalFitness = prefixSum[individuals.Count];
        var selectedIndividuals = new List<Individual<TChromosome>>();
        for (var i = 0; i < populationSize; i++)
        {
            var probability = Random.Shared.NextDouble() * totalFitness;
            var index = Population<TChromosome>.BinarySearchMaxValueIndexLessThan(prefixSum, probability);
            selectedIndividuals.Add(new(individuals[index]));
        }
        individuals = selectedIndividuals;
    }

    public void CrossOver()
    {
        var extendingIndividuals = new List<Individual<TChromosome>>();
        for (var i = 0; i < individuals.Count; i++)
        {
            var individual = individuals[i];
            var probability = Random.Shared.NextDouble();
            if (probability <= crossOverProbability)
            {
                var alice = new Individual<TChromosome>(individual);
                var bobIndex = Random.Shared.Next(individuals.Count);
                while (bobIndex == i)
                {
                    bobIndex = Random.Shared.Next(individuals.Count);
                }
                var bob = new Individual<TChromosome>(individuals[bobIndex]);
                var crossOverStartpoint = Random.Shared.Next(IChromosome.Capacity);
                for (var j = crossOverStartpoint; j < IChromosome.Capacity; j++)
                {
                    (alice[j], bob[j]) = (bob[j], alice[j]);
                }
                extendingIndividuals.Add(alice);
                extendingIndividuals.Add(bob);
            }
        }
        individuals.AddRange(extendingIndividuals);
    }

    public void Mutate()
    {
        var extendingIndividuals = new List<Individual<TChromosome>>();
        foreach (var individual in individuals)
        {
            var probability = Random.Shared.NextDouble();
            if (probability <= mutationProbability)
            {
                var neoIndividual = new Individual<TChromosome>(individual);
                var mutatePoint = Random.Shared.Next(IChromosome.Capacity);
                neoIndividual[mutatePoint] = (neoIndividual[mutatePoint] + 1) % 2;
                extendingIndividuals.Add(neoIndividual);
            }
        }
        individuals.AddRange(extendingIndividuals);
    }

    public void CheckAndForceMutate()
    {
        var onesInChromosome = new int[IChromosome.Capacity];
        foreach (var individual in individuals)
        {
            for (var i = 0; i < IChromosome.Capacity; i++)
            {
                onesInChromosome[i] += individual[i];
            }
        }

        var forceMutatePoints = new List<int>();
        for (var i = 0; i < onesInChromosome.Length; i++)
        {
            if (onesInChromosome[i] == 0 || onesInChromosome[i] == individuals.Count)
            {
                forceMutatePoints.Add(i);
            }
        }

        if (forceMutatePoints.Count != 0)
        {
            Log.ForcedMutation($"There's no diversity in {forceMutatePoints.CollectAsString()}, forcing to mutate.");
            var extendingIndividuals = new List<Individual<TChromosome>>();
            foreach (var individual in individuals)
            {
                var neoIndividual = new Individual<TChromosome>(individual);
                foreach (var point in forceMutatePoints)
                {
                    var mutation = Random.Shared.Next() % 2 != 0;
                    if (mutation)
                    {
                        neoIndividual[point] = (neoIndividual[point] + 1) % 2;
                    }
                }
                extendingIndividuals.Add(neoIndividual);
            }
            individuals.AddRange(extendingIndividuals);
        }
    }

    public List<Individual<TChromosome>> StopEvolution()
    {
        individuals.Sort(new Comparer());
        return individuals;
    }

    static int BinarySearchMaxValueIndexLessThan(IEnumerable<double> sequence, double target)
    {
        var left = 0;
        var right = sequence.Count() - 1;
        var result = 0;
        while (left <= right)
        {
            var middle = (left + right) / 2;
            var candidate = sequence.ElementAt(middle);
            if (candidate < target)
            {
                result = middle;
                left = middle + 1;
            }
            else if (candidate > target) right = middle - 1;
            else return middle;
        }
        return result;
    }
}
