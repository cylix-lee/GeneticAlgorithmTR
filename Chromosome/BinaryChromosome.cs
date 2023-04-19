using System.Text;

namespace GeneticAlgorithmTR.Chromosome;

class BinaryChromosome : IChromosome
{
    public double DoubleValue { get; private set; }
    public int this[int i]
    {
        get => code[i];
        set
        {
            code[i] = value;
            DoubleValue = ChromosomeUtil.CalculateBinary(code);
        }
    }

    readonly int[] code = new int[IChromosome.Capacity];

    public BinaryChromosome()
    {
        for (var i = 0; i < IChromosome.Capacity; i++)
        {
            code[i] = Random.Shared.Next() % 2;
        }
        DoubleValue = ChromosomeUtil.CalculateBinary(code);
    }

    public BinaryChromosome(BinaryChromosome another)
    {
        for (var i = 0; i < IChromosome.Capacity; i++)
        {
            code[i] = another.code[i];
        }
        DoubleValue = another.DoubleValue;
    }

    public static bool operator ==(BinaryChromosome? left, BinaryChromosome? right)
        => ReferenceEquals(left, right) || (left != null && right != null && left.Equals(right));
    public static bool operator !=(BinaryChromosome? left, BinaryChromosome? right) => !(left == right);

    public override bool Equals(object? obj) => obj != null && obj is BinaryChromosome other && Equals(other);
    public override int GetHashCode() => code.GetHashCode();

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var digit in code)
        {
            stringBuilder.Append(digit);
        }
        return $"{nameof(BinaryChromosome)} {{ {stringBuilder}, {nameof(DoubleValue)} = {DoubleValue} }}";
    }

    public bool Equals(BinaryChromosome? other)
    {
        if (other == null) return false;
        for (var i = 0; i < IChromosome.Capacity; i++)
        {
            if (code[i] != other.code[i]) return false;
        }
        return true;
    }

    public object Clone() => new BinaryChromosome(this);
}
