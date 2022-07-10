namespace SolvableBlazor.Objects;

class Session
{
    public MathProblem Problem { get; private set; }

    Random random = new Random(DateTime.Now.Millisecond);

    public User CurrentUser { get; set; } = new User();
    public Session()
    {
        CreateMathProblem();
    }

    public void CreateMathProblem()
    {
        Problem = new MathProblem(random.Next(5, 10), random.Next(2, 4));
    }
}
