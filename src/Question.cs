namespace QuickMathCLI;

public enum Operation
{
    Add,
    Subtract,
    Multiply
}

public record Difficulty(string Name, int Min, int Max)
{
    public static readonly Difficulty SuperEasy = new("Super Easy", 1, 10);
    public static readonly Difficulty Easy = new("Easy", 1, 25);
    public static readonly Difficulty Medium = new("Medium", 1, 50);
    public static readonly Difficulty Hard = new("Hard", 1, 100);
    public static readonly Difficulty Expert = new("Expert", 1, 500);

    public static Difficulty[] Presets => [SuperEasy, Easy, Medium, Hard, Expert];
}

public class Question
{
    private static readonly Random _random = new();

    public int Left { get; }
    public int Right { get; }
    public Operation Operation { get; }
    public int Answer { get; }

    public string Display => $" {Left} {OpSymbol} {Right}";
    public string AnswerString => Answer.ToString();
    public char OpSymbol => Operation switch
    {
        Operation.Add => '+',
        Operation.Subtract => '-',
        Operation.Multiply => '*',
        _ => '?'
    };

    private Question(int left, int right, Operation op, int answer)
    {
        Left = left;
        Right = right;
        Operation = op;
        Answer = answer;
    }

    public static Question Generate(Operation[]? allowedOps = null, Difficulty? difficulty = null)
    {
        var ops = allowedOps ?? [Operation.Add, Operation.Subtract, Operation.Multiply];
        var diff = difficulty ?? Difficulty.Medium;
        var op = ops[_random.Next(ops.Length)];

        return op switch
        {
            Operation.Add => GenerateAddition(diff.Min, diff.Max),
            Operation.Subtract => GenerateSubtraction(diff.Min, diff.Max),
            Operation.Multiply => GenerateMultiplication(diff.Min, diff.Max),
            _ => GenerateAddition(diff.Min, diff.Max)
        };
    }

    private static Question GenerateAddition(int min, int max)
    {
        int a = _random.Next(min, max + 1);
        int b = _random.Next(min, max + 1);
        return new Question(a, b, Operation.Add, a + b);
    }

    private static Question GenerateSubtraction(int min, int max)
    {
        int a = _random.Next(min + 1, max + 1);
        int b = _random.Next(min, a);
        return new Question(a, b, Operation.Subtract, a - b);
    }

    private static Question GenerateMultiplication(int min, int max)
    {
        int cap = Math.Min(max, 20);
        int a = _random.Next(min, cap + 1);
        int b = _random.Next(min, cap + 1);
        return new Question(a, b, Operation.Multiply, a * b);
    }
}
