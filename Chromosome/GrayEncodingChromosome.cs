using System.Text;

namespace GeneticAlgorithmTR.Chromosome;

class GrayEncodingChromosome : IChromosome
{
    public double DoubleValue { get; private set; }
    public int this[int i]
    {
        get => code[i];
        set
        {
            code[i] = value;
            DoubleValue = ChromosomeUtil.CalculateGray(code);
        }
    }

    readonly int[] code = new int[IChromosome.Capacity];

    public GrayEncodingChromosome()
    {
        for (var i = 0; i < IChromosome.Capacity; i++)
        {
            code[i] = Random.Shared.Next() % 2;
        }
        DoubleValue = ChromosomeUtil.CalculateGray(code);
    }

    public GrayEncodingChromosome(GrayEncodingChromosome another)
    {
        for (var i = 0; i < IChromosome.Capacity; i++)
        {
            code[i] = another.code[i];
        }
        DoubleValue = another.DoubleValue;
    }

    public static bool operator ==(GrayEncodingChromosome? left, GrayEncodingChromosome? right)
        => ReferenceEquals(left, right) || (left != null && right != null && left.Equals(right));
    public static bool operator !=(GrayEncodingChromosome? left, GrayEncodingChromosome? right) => !(left == right);

    public override bool Equals(object? obj) => obj != null && obj is GrayEncodingChromosome other && Equals(other);
    public override int GetHashCode() => code.GetHashCode();

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var digit in code)
        {
            stringBuilder.Append(digit);
        }
        return $"{nameof(GrayEncodingChromosome)} {{ {stringBuilder}, {nameof(DoubleValue)} = {DoubleValue} }}";
    }

    public bool Equals(GrayEncodingChromosome? other)
    {
        if (other == null) return false;
        for (var i = 0; i < IChromosome.Capacity; i++)
        {
            if (code[i] != other.code[i]) return false;
        }
        return true;
    }

    public object Clone() => new GrayEncodingChromosome(this);
}
