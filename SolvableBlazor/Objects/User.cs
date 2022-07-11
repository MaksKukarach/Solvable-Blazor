using System.ComponentModel.DataAnnotations.Schema;

namespace SolvableBlazor.Objects;

public class User
{
    // так как объекты этого класса будут использованы в бд, то первое свойство
    // должно быть int, чтобы на его основе бд могла создать первичный ключ
    public int Id { get; private set; }
    public int Exp { get; set; } = 0;
    public int Level { get; set; } = 1;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "Unassigned";

    [NotMapped]
    public MathProblem Problem { get; set; } = new MathProblem();

    public void CreateMathProblem()
    {
        Problem = new MathProblem();
    }
}
