using QuickMathCLI;
using Spectre.Console;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

bool playing = true;

while (playing)
{
    Console.Clear();
    AnsiConsole.Write(new FigletText("QuickMath").Color(Color.Cyan1));
    AnsiConsole.Write(new Rule("[yellow]Fast Math Challenge[/]") { Style = new Style(Color.Grey) });
    Console.WriteLine();

    //  Game Mode selector thing
    var modeChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select [green]Game Mode[/]")
            .PageSize(5)
            .HighlightStyle(new Style(Color.Cyan1, null, Decoration.Bold))
            .AddChoices("Standard (20 questions)", "Infinite (endless)", "Timed (60s)"));

    var gameMode = modeChoice switch
    {
        "Standard (20 questions)" => GameMode.Standard,
        "Infinite (endless)" => GameMode.Infinite,
        "Timed (60s)" => GameMode.Timed,
        _ => GameMode.Standard
    };

    Console.WriteLine();

    //idk how thath called, the thing where you can select multiple options, so I just called it opSelections; i think its for Operation options, but i dont know if that is the correct term for it. anyway, this is where you select the operations you want to practice. if you select none, it will default to all operations.

    var opSelections = AnsiConsole.Prompt(
        new MultiSelectionPrompt<string>()
            .Title("Select [green]Operations[/] (space to toggle)")
            .PageSize(5)
            .HighlightStyle(new Style(Color.Cyan1, null, Decoration.Bold))
            .InstructionsText("[grey](Press [cyan]space[/] to toggle, [green]enter[/] to confirm)[/]")
            .AddChoices("Addition", "Subtraction", "Multiplication"));

    var allowedOps = opSelections.Count == 0
        ? new[] { Operation.Add, Operation.Subtract, Operation.Multiply }
        : opSelections.Select(s => s switch
        {
            "Addition" => Operation.Add,
            "Subtraction" => Operation.Subtract,
            "Multiplication" => Operation.Multiply,
            _ => Operation.Add
        }).ToArray();

    Console.WriteLine();

    //  Difficulty  selector thing
    var diffChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select [green]Difficulty[/]")
            .PageSize(6)
            .HighlightStyle(new Style(Color.Cyan1, null, Decoration.Bold))
            .AddChoices("Super Easy (1-10)", "Easy (1-25)", "Medium (1-50)", "Hard (1-100)", "Expert (1-500)", "Custom"));

    Difficulty difficulty;
    if (diffChoice == "Custom")
    {
        Console.WriteLine();
        var min = AnsiConsole.Ask<int>("Enter [green]minimum[/] number:");
        var max = AnsiConsole.Ask<int>("Enter [green]maximum[/] number:");
        difficulty = min > max
            ? new Difficulty("Custom", max, min)
            : new Difficulty("Custom", min, max);
    }
    else
    {
        difficulty = diffChoice switch
        {
            "Super Easy (1-10)" => Difficulty.SuperEasy,
            "Easy (1-25)" => Difficulty.Easy,
            "Medium (1-50)" => Difficulty.Medium,
            "Hard (1-100)" => Difficulty.Hard,
            "Expert (1-500)" => Difficulty.Expert,
            _ => Difficulty.Medium
        };
    }

    //  Setup 
    var game = new Backend
    {
        Mode = gameMode,
        QuestionCount = gameMode == GameMode.Standard ? 20 : int.MaxValue,
        Lives = gameMode == GameMode.Timed ? int.MaxValue : 3,
        TimeLimitSeconds = 60
    };

    //  Summary 
    Console.Clear();
    AnsiConsole.Write(new FigletText("QuickMath").Color(Color.Cyan1));
    AnsiConsole.Write(new Rule("[yellow]Ready![/]") { Style = new Style(Color.Grey) });
    Console.WriteLine();

    var summary = new Panel(
        new Markup(
            $"Game Mode:  [bold]{gameMode}[/]\n" +
            $"Operations: [bold]{string.Join(", ", allowedOps.Select(OpName))}[/]\n" +
            $"Difficulty: [bold]{difficulty.Name}[/]  [dim]({difficulty.Min}-{difficulty.Max})[/]"))
    {
        Border = BoxBorder.Rounded,
        Padding = new Padding(3, 1, 3, 1)
    };
    AnsiConsole.Write(summary);
    Console.WriteLine();

    AnsiConsole.MarkupLine("Press [green]ENTER[/] to start, [yellow]ESC[/] to quit...");
    var startKey = Console.ReadKey(true);
    if (startKey.Key == ConsoleKey.Escape) return;

    // Game Loop 
    var startTime = Stopwatch.StartNew();
    int questionNum = 1;

    while (true)
    {
        if (game.GameOver) break;
        if (gameMode == GameMode.Standard && questionNum > game.QuestionCount) break;
        if (gameMode == GameMode.Timed && startTime.Elapsed.TotalSeconds >= game.TimeLimitSeconds) break;

        Console.Clear();
        AnsiConsole.Write(new FigletText("QuickMath").Color(Color.Cyan1));
        AnsiConsole.Write(new Rule("[yellow]Fast Math Challenge[/]") { Style = new Style(Color.Grey) });
        Console.WriteLine();

        var q = Question.Generate(allowedOps, difficulty);

        var stats = $"Question [bold]{questionNum}[/]/{FormatCount(game.QuestionCount)}  " +
                     $"Score: [green]{game.Score}[/]";
        if (gameMode != GameMode.Timed)
            stats += $"  Streak: [yellow]{game.Streak}[/]  Lives: [maroon]{new string('\u2665', game.Lives)}[/]";
        else
            stats += $"  Time: [cyan]{game.TimeLimitSeconds - (int)startTime.Elapsed.TotalSeconds}s[/]";
        stats += $"  Accuracy: [blue]{game.Accuracy:F0}%[/]";

        AnsiConsole.MarkupLine(stats);
        Console.WriteLine();

        var panel = new Panel(
            new Markup($"[bold yellow][/] {q.Left} [green]{q.OpSymbol}[/] {q.Right} [cyan]=[/]"))
            .Border(BoxBorder.Rounded)
            .Padding(2, 1);
        AnsiConsole.Write(panel);
        Console.WriteLine();

        int inputLeft = 4;
        int inputTop = Console.CursorTop;
        Console.SetCursorPosition(inputLeft, inputTop);
        AnsiConsole.Markup("[dim]>> [/]");
        int cursorStart = inputLeft + 4;
        Console.SetCursorPosition(cursorStart, inputTop);

        string input = "";
        while (true)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Console.CursorVisible = true;
                Console.Clear();
                AnsiConsole.MarkupLine("[yellow]Game aborted.[/]");
                return;
            }
            else if (char.IsDigit(key.KeyChar))
            {
                input += key.KeyChar;
            }
            else
            {
                continue;
            }

            Console.SetCursorPosition(cursorStart, inputTop);
            Console.Write(new string(' ', Console.WindowWidth - cursorStart));
            Console.SetCursorPosition(cursorStart, inputTop);
            Console.Write(input);

            string ans = q.AnswerString;
            if (input == ans) break;
            if (input.Length > 0 && !ans.StartsWith(input)) break;
        }

        bool correct = game.SubmitAnswer(int.Parse(input), q.Answer);

        Console.SetCursorPosition(cursorStart, inputTop);
        Console.Write(new string(' ', Console.WindowWidth - cursorStart));
        Console.SetCursorPosition(cursorStart, inputTop);

        if (correct)
        {
            AnsiConsole.MarkupLine($"[green]{input}  \u2713 Correct![/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]{input}  \u2717 Wrong! Answer was [bold]{q.Answer}[/][/]");
            if (game.Lives <= 0 && gameMode != GameMode.Timed)
            {
                Console.WriteLine();
                AnsiConsole.MarkupLine("[red bold]Game Over! You ran out of lives![/]");
            }
        }

        Thread.Sleep(correct ? 400 : 1200);
        questionNum++;
    }

    var elapsed = startTime.Elapsed;
    Console.Clear();
    AnsiConsole.Write(new FigletText("Done!").Color(Color.Green1));
    AnsiConsole.Write(new Rule());
    Console.WriteLine();
    AnsiConsole.MarkupLine(
        $"Final Score: [bold green]{game.Score}[/]/{game.Total}  " +
        $"Accuracy: [blue]{game.Accuracy:F0}%[/]  " +
        $"Best Streak: [yellow]{game.BestStreak}[/]  " +
        $"Time: [cyan]{elapsed.TotalSeconds:F1}s[/]");
    Console.WriteLine();

    AnsiConsole.Markup("Play again? [green]Y[/]es / [red]N[/]o: ");
    Console.CursorVisible = true;
    var again = Console.ReadKey(true);
    playing = again.Key == ConsoleKey.Y;
}

static string FormatCount(int n) => n == int.MaxValue ? "\u221e" : n.ToString();

static string OpName(Operation op) => op switch
{
    Operation.Add => "Addition",
    Operation.Subtract => "Subtraction",
    Operation.Multiply => "Multiplication",
    _ => "?"
};
