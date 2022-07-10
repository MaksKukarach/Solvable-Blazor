namespace SolvableBlazor.Objects;

class MathProblem
{
    readonly bool isEquation;

    readonly int numberOfElements;

    static readonly int upperBorder = 64;
    public static int UpperBorder { get { return upperBorder; } }

    static readonly int lowerBorder = 1;
    public static int LowerBorder { get { return lowerBorder; } }

    int answer;
    public int Answer { get { return answer; } }

    int score;
    public int Score { get { return score; } }

    string possibleOperations;

    string problem = "";
    public string StringValue { get { return problem; } }

    List<string> problemList = new List<string> { " " };

    Random random = new Random(DateTime.Now.Millisecond);

    /// <summary>
    /// Set an answer, number of elements, and whether you need an equation
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="numberOfElements"></param>
    /// <param name="isEquation"></param>
    public MathProblem(int answer, int numberOfElements, bool isEquation = false)
    {
        this.answer = answer;
        this.numberOfElements = numberOfElements;
        this.isEquation = isEquation;

        CreateProblem();
        if (isEquation) TransformIntoEquation();
        CalculateScore();

        problem = String.Join("", problemList);
    }

    private void CreateProblem()
    {
        problemList.Add($"{answer}");
        problemList.Add("");

        for (int i = 2; i <= numberOfElements; i++)
        {
            while (true)  // picking random elements until it's a number
            {
                int index = random.Next(0, problemList.Count);
                string pickedElement = problemList[index];

                if (int.TryParse(pickedElement, out int n))
                {
                    int number = Convert.ToInt32(pickedElement);
                    if (number > 1)
                    {
                        List<string> result;

                        possibleOperations = "+-";

                        if (FindDivisors(number).Count != 0) possibleOperations += "*";
                        if (number <= upperBorder) possibleOperations += "::";

                        result = RepresentRandomly(number, problemList[index - 1], problemList[index + 1]);

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
    }

    private void TransformIntoEquation()
    {
        int temp = answer;

        while (true)
        {
            int i = random.Next(problemList.Count);

            if (int.TryParse(problemList[i], out int number))
            {
                answer = number;
                problemList[i] = "x";
                break;
            }

        }

        problemList.Add(" = ");
        problemList.Add($"{temp}");
    }

    private List<string> RepresentRandomly(int number, string symbolBefore, string symbolAfter)
    {
        char operation = possibleOperations[random.Next(possibleOperations.Length)];
        switch (operation)
        {
            case '+':
                return RepresentAsSum(number, symbolBefore, symbolAfter);
            case '-':
                return RepresentAsDifference(number, symbolBefore, symbolAfter);
            case '*':
                return RepresentAsMultiplication(number, FindDivisors(number));
            case ':':
                return RepresentAsDivision(number, symbolBefore);
        }

        return new List<string>();
    }

    private List<string> RepresentAsSum(int number, string symbolBefore, string symbolAfter)
    {
        int first = random.Next(lowerBorder, number);
        int second = number - first;
        if (AreParenthesesRequired(symbolBefore, symbolAfter))
            return new List<string> { "(", $"{first}", "+", $"{second}", ")" };
        return new List<string> { $"{first}", "+", $"{second}" };
    }

    private List<string> RepresentAsDifference(int number, string symbolBefore, string symbolAfter)
    {
        int first = random.Next(lowerBorder, number);
        int second = number + first;
        if (AreParenthesesRequired(symbolBefore, symbolAfter))
            return new List<string> { "(", $"{second}", "-", $"{first}", ")" };
        return new List<string> { $"{second}", "-", $"{first}" };
    }

    private List<string> RepresentAsMultiplication(int number, List<int> divisors)
    {
        int first = divisors[random.Next(divisors.Count)];
        int second = number / first;
        return new List<string> { $"{first}", "*", $"{second}" };
    }

    private List<string> RepresentAsDivision(int number, string symbolBefore)
    {
        int first = random.Next(lowerBorder + 1, 11 - (int)Math.Sqrt(number));
        int second = number * first;
        if (symbolBefore == ":")
            return new List<string> { "(", $"{second}", ":", $"{first}", ")" };
        return new List<string> { $"{second}", ":", $"{first}" };
    }

    private List<int> FindDivisors(int number)
    {
        List<int> divisors = new List<int>();
        for (int i = 2; i * i <= Math.Sqrt(number); i++)
        {
            if ((number % i == 0))
            {
                divisors.Add(i);
                if (i * i != number)
                    divisors.Add(number / i);
            }
        }
        return divisors;
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
                if (n < 10) score += 1;
                else if (n < 100) score += 2;
                else score += 3;
            }
        }

        if (isEquation) score *= 2;
    }

    public void PrintInfo()
    {
        throw new NotImplementedException();
    }
}
