namespace SolvableBlazor.Objects;

public class User
{
    public static readonly Dictionary<int, (int[], int[], string, int)> LevelsMap = new()
    {
        { 1, (new[] { 5, 10 }, new[] { 2, 3 }, "+", 20) },
        { 2, (new[] { 5, 10 }, new[] { 2, 3 }, "+-", 20) },
        { 3, (new[] { 5, 10 }, new[] { 2, 4 }, "+-*", 50) },
        { 4, (new[] { 5, 10 }, new[] { 2, 4 }, "+-*:", 75) },
        { 5, (new[] { 10, 20 }, new[] { 3, 5 }, "+-", 75) },
        { 6, (new[] { 10, 20 }, new[] { 3, 5 }, "+-*:", 75) },

        { 7, (new[] { 20, 50 }, new[] { 2, 3 }, "+-", 100) },
        { 8, (new[] { 20, 50 }, new[] { 2, 3 }, "+-*:", 100) },
        { 9, (new[] { 20, 50 }, new[] { 3, 5 }, "+-*:", 100) },
        { 10, (new[] { 20, 50 }, new[] { 3, 5 }, "+-*:", 100) },
        { 11, (new[] { 20, 50 }, new[] { 3, 5 }, "+-*:", 100) },
        { 12, (new[] { 10, 20 }, new[] { 3, 6 }, "+-*:", 125) },

        { 13, (new[] { 20, 100 }, new[] { 2, 3 }, "+-", 125) },
        { 14, (new[] { 20, 100 }, new[] { 2, 3 }, "+-*:", 125) },
        { 15, (new[] { 20, 100 }, new[] { 3, 6 }, "+-*:", 125) },
        { 16, (new[] { 20, 100 }, new[] { 3, 6 }, "+-*:", 125) },
        { 17, (new[] { 20, 50 }, new[] { 4, 7 }, "+-*:", 150) },

        { 18, (new[] { 30, 150 }, new[] { 2, 3 }, "+-*:", 150) },
        { 19, (new[] { 20, 50 }, new[] { 4, 7 }, "+-*:", 150) },
        { 20, (new[] { 20, 50 }, new[] { 4, 7 }, "+-*:", 150) },
        { 21, (new[] { 20, 50 }, new[] { 4, 7 }, "+-*:", 150) },
        { 22, (new[] { 20, 50 }, new[] { 4, 7 }, "+-*:", 150) },
    };

    // так как объекты этого класса будут использованы в бд, то первое свойство
    // должно быть int, чтобы на его основе бд могла создать первичный ключ
    public int Id { get; private set; }
    public int Exp { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int ExpForNextLevel { get; set; } = 20;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "Unassigned";

    public string Problem { get; set; } = "";
    public int ProblemAnswer { get; set; }
    public int ProblemScore { get; set; }

    public void CreateMathProblem()
    {
        int tempLevel;
        if (Level > LevelsMap.Count) tempLevel = LevelsMap.Count;
        else tempLevel = Level;

        Random random = new Random(DateTime.Now.Millisecond);
        var (answerBorder, numberOfElementsBorder, operations, tmp) = LevelsMap[tempLevel];

        int answer = random.Next(answerBorder[0], answerBorder[1]);
        int numberOfElements = random.Next(numberOfElementsBorder[0], numberOfElementsBorder[1]);

        MathProblem p = new MathProblem(answer, numberOfElements, operations);

        Problem = p.ToString();
        ProblemAnswer = p.Answer;
        ProblemScore = p.Score;
    }
}
