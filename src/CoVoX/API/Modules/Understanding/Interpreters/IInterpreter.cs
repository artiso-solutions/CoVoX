namespace Covox.Understanding
{
    internal interface IInterpreter
    {
        double CalculateMatchScore(string target, string input);
    }
}
