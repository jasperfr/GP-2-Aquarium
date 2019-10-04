using Aquarium.AI.Fuzzy_Logic.Fuzzy_Terms;
using System;
using System.Collections.Generic;

namespace Aquarium.AI.Fuzzy_Logic
{
    public class FuzzyModule
    {
        public enum DefuzzifyType
        {
            MAX_AV,
            CENTROID
        };

        public const int CentroidSamples = 15;

        public Dictionary<string, FuzzyVariable> VarMap = new Dictionary<string, FuzzyVariable>();
        public List<FuzzyRule> Rules = new List<FuzzyRule>();

        public FuzzyVariable CreateFLV(string name)
        {
            FuzzyVariable fuzzyvar = new FuzzyVariable(name);
            VarMap.Add(name, fuzzyvar);
            return fuzzyvar;
        }

        public void AddRule(IFuzzyTerm antecedent, IFuzzyTerm consequence)
        {
            Rules.Add(new FuzzyRule(antecedent, consequence.Clone()));
        }

        public void Fuzzify(string flvName, ref double value)
        {
            VarMap[flvName].Fuzzify(ref value);
        }

        private void SetConfidencesOfConsequentsToZero()
        {
            Rules.ForEach(rule => rule.Consequence.ClearDOM());
        }

        public double Defuzzify(string flvName, DefuzzifyType type)
        {
            // Clear DOMs of all consequents.
            SetConfidencesOfConsequentsToZero();

            for (int i = 0; i < Rules.Count; i++)
            {
                Console.WriteLine($"{Rules[i]}");
                Rules[i].Calculate();
            }

            Console.WriteLine();

            // custom code - get maximum consequences of ruleset
            // it's trash though, fix later
            // uses the MaxAV method using pure garbage code that.. works
            // pointers and C# just don't mix...

            List<OutputSet> outputSet = new List<OutputSet>();

            for (int i = 0; i < Rules.Count; i++)
            {
                FuzzyTermSet consequence = (FuzzyTermSet) Rules[i].Consequence;
                string name = consequence.VariableName;
                double value = consequence.GetDOM();
                double represents = consequence.Set.Represents;

                if(outputSet.Exists(os => os.Name == name))
                {
                    outputSet.Find(os => os.Name == name).SetMax(value);
                }
                else
                {
                    outputSet.Add(new OutputSet(name, value, represents));
                }
            }

            double calculatedMaxAV = 0.0;
            double divisor = 0.0;
            foreach(OutputSet set in outputSet)
            {
                calculatedMaxAV += (set.RepresentativeValue * set.Value);
                divisor += set.Value;
            }
            calculatedMaxAV /= divisor;
            return calculatedMaxAV;
        }
    }

    class OutputSet
    {
        public string Name;
        public double Value;
        public double RepresentativeValue;

        public OutputSet(string name, double value, double represents)
        {
            Name = name;
            Value = value;
            RepresentativeValue = represents;
        }

        public void SetMax(double value)
        {
            if (Value < value) Value = value;
        }
    }
}
