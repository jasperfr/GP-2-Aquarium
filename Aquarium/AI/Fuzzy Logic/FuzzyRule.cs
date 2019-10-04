using Aquarium.AI.Fuzzy_Logic.Fuzzy_Terms;
using System;

namespace Aquarium.AI.Fuzzy_Logic
{
    public class FuzzyRule
    {
        public IFuzzyTerm Antecedent;
        public IFuzzyTerm Consequence;

        public FuzzyRule(IFuzzyTerm antecedent, IFuzzyTerm consequence)
        {
            Antecedent = antecedent;
            Consequence = consequence;
        }

        public void Calculate()
        {
            Consequence.ORwithDOM(Antecedent.GetDOM());
        }

        public override string ToString()
        {
            string output = "Ruleset - ";
            output += Antecedent.ToString() + " = " + Consequence.ToString();
            return output;
        }
    }
}
