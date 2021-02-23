using System.Collections.Generic;

namespace API.Understanding
{
    internal interface IInterpreter
    {
        double CalculateMatchScore(string target, string input);
    }
}
