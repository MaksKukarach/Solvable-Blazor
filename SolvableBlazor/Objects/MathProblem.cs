namespace SolvableBlazor.Objects;

public class MathProblem
{
    #region Properties

    public readonly bool IsEquation = false;

    public readonly int NumberOfElements;

    public readonly int MaxValue = 64;

    public readonly int MinValue = 1;

    public int Answer { get; private set; }

    public int Score { get; private set; }

    public string Operations { get; private set; } = "";

    string problemStr = "";

    List<string> problemList = new List<string> { " " };

    Random random = new Random(DateTime.Now.Millisecond);

    #endregion

    #region Constructors

    /// <summary>
    /// Create random simple problem
    /// </summary>
    public MathProblem()
    {
        Answer = random.Next(MinValue, 10);
        NumberOfElements = random.Next(2, 4);
        Operations = "+-*:";

        CreateProblem();
    }

    /// <summary>
    /// Create custom problem
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="numberOfElements"></param>
    /// <param name="isEquation"></param>
    public MathProblem(int answer, int numberOfElements = 2, string operations = "+-*:", bool isEquation = false)
    {
        if (numberOfElements > answer)
        {
            throw new ArgumentException("Number of elements must not exceed the answer");
        }
        else
        {
            Answer = answer;
            NumberOfElements = numberOfElements;
            Operations = operations;
            IsEquation = isEquation;

            CreateProblem();
        }
    }

    #endregion

    #region Methods

    private void CreateProblem()
    {
        problemList.Add(Convert.ToString(Answer));
        problemList.Add("");

        for (int i = 1; i < NumberOfElements; i++)
        {
            // picking random elements until it's a number
            while (true)
            {
                int index = random.Next(0, problemList.Count);
                string pickedElement = problemList[index];

                if (int.TryParse(pickedElement, out int number))
                {
                    if (number != 1)
                    {
                        List<string> result = RepresentRandomly(number, problemList[index - 1], problemList[index + 1]);

                        problemList.RemoveAt(index);

                        List<string> firstHalf = problemList.GetRange(0, index);
                        List<string> secondHalf = problemList.GetRange(index, problemList.Count - index);

                        firstHalf.AddRange(result);
                        firstHalf.AddRange(secondHalf);

                        problemList = firstHalf;

                        break;
                    }
                }
            }

        }

        if (IsEquation) TransformIntoEquation();
        CalculateScore();

        problemStr = String.Join("", problemList);
    }

    private List<string> RepresentRandomly(int number, string symbolBefore, string symbolAfter)
    {
        string allowedOperations = "";

        if (Operations.Contains('+'))
            allowedOperations += "+";

        if (Operations.Contains('-'))
            allowedOperations += "-";

        if (Operations.Contains('*') && (GetDivisors(number).Length != 0))
            allowedOperations += "*";

        if (Operations.Contains(':') && (number <= MaxValue))
            allowedOperations += "::";

        char operation = allowedOperations[random.Next(allowedOperations.Length)];

        switch (operation)
        {
            case '+':
                return RepresentAsSum(number, symbolBefore, symbolAfter);
            case '-':
                return RepresentAsDifference(number, symbolBefore, symbolAfter);
            case '*':
                return RepresentAsMultiplication(number, GetDivisors(number));
            case ':':
                return RepresentAsDivision(number, symbolBefore);
        }

        return new List<string>();
    }

    private List<string> RepresentAsSum(int number, string symbolBefore, string symbolAfter)
    {
        int first = random.Next(MinValue, number);
        int second = number - first;
        if (AreParenthesesRequired(symbolBefore, symbolAfter))
            return new List<string> { "(", $"{first}", "+", $"{second}", ")" };
        return new List<string> { $"{first}", "+", $"{second}" };
    }

    private List<string> RepresentAsDifference(int number, string symbolBefore, string symbolAfter)
    {
        int first = random.Next(MinValue, number);
        int second = number + first;
        if (AreParenthesesRequired(symbolBefore, symbolAfter))
            return new List<string> { "(", $"{second}", "-", $"{first}", ")" };
        return new List<string> { $"{second}", "-", $"{first}" };
    }

    private List<string> RepresentAsMultiplication(int number, int[] divisors)
    {
        int first = divisors[random.Next(divisors.Length)];
        int second = number / first;
        return new List<string> { $"{first}", "*", $"{second}" };
    }

    private List<string> RepresentAsDivision(int number, string symbolBefore)
    {
        int first = random.Next(MinValue + 1, 11 - (int)Math.Sqrt(number));
        int second = number * first;
        if (symbolBefore == ":")
            return new List<string> { "(", $"{second}", ":", $"{first}", ")" };
        return new List<string> { $"{second}", ":", $"{first}" };
    }

    /// <summary>
    /// Finds all the divisors of any positive integer passed as argument. 
    /// Returns a list of int with all the divisors of the argument.
    /// </summary>
    public static int[] GetDivisors(int n)
    {
        List<int> divisors = new List<int>();
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
            {
                divisors.Add(i);
                if (i != n / i)
                {
                    divisors.Add(n / i);
                }
            }
        }
        divisors.Sort();
        return divisors.ToArray();
    }

    private void TransformIntoEquation()
    {
        int temp = Answer;

        while (true)
        {
            int i = random.Next(problemList.Count);

            if (int.TryParse(problemList[i], out int number))
            {
                Answer = number;
                problemList[i] = "x";
                break;
            }

        }

        problemList.Add(" = ");
        problemList.Add($"{temp}");
    }

    private bool AreParenthesesRequired(string symbolBefore, string symbolAfter)
    {
        string[] specialSymbols = { "*", ":" };
        return (specialSymbols.Contains(symbolBefore) || specialSymbols.Contains(symbolAfter) || (symbolBefore == "-"));
    }

    private void CalculateScore()
    {
        foreach (string element in problemList)
        {
            if (int.TryParse(element, out int n))
            {
                if (n < 10) Score += 1;
                else if (n < 100) Score += 2;
                else Score += 3;
            }
        }

        if (IsEquation) Score *= 2;
    }

    public override string ToString()
    {
        return problemStr;
    }

    #endregion
}
