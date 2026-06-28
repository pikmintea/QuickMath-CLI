namespace QuickMathCLI;

public enum GameMode
{
    Standard,
    Infinite,
    Timed
}

public class Backend
{
    public GameMode Mode { get; set; } = GameMode.Standard;
    public int Score { get; private set; }
    public int Total { get; private set; }
    public int Streak { get; private set; }
    public int BestStreak { get; private set; }
    public int Lives { get; set; } = 3;
    public int QuestionCount { get; set; } = 20;
    public int TimeLimitSeconds { get; set; } = 60;

    public double Accuracy => Total > 0 ? (double)Score / Total * 100 : 0;
    public bool GameOver => Mode != GameMode.Timed && Lives <= 0;

    public bool SubmitAnswer(int userAnswer, int correctAnswer)
    {
        Total++;
        if (userAnswer == correctAnswer)
        {
            Score++;
            Streak++;
            if (Streak > BestStreak) BestStreak = Streak;
            return true;
        }
        else
        {
            Streak = 0;
            if (Mode != GameMode.Timed) Lives--;
            return false;
        }
    }
}
