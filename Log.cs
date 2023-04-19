namespace GeneticAlgorithmTR;

static class Log
{
    record Tag(string Text, ConsoleColor Color);

    static readonly Tag OkTag = new($"{nameof(Ok)}            ", ConsoleColor.Green);
    static readonly Tag InfoTag = new($"{nameof(Info)}          ", ConsoleColor.Blue);
    static readonly Tag ForcedMutationTag = new(nameof(ForcedMutation), ConsoleColor.Red);

    static void LogWithTag(Tag tag, string message)
    {
        var originalColor = Console.ForegroundColor;
        {
            Console.ForegroundColor = tag.Color;
            Console.Write(tag.Text);
        }
        Console.ForegroundColor = originalColor;
        Console.WriteLine($" {message}");
    }

    public static void Ok(string message) => LogWithTag(OkTag, message);
    public static void Info(string message) => LogWithTag(InfoTag, message);
    public static void ForcedMutation(string message) => LogWithTag(ForcedMutationTag, message);
}
