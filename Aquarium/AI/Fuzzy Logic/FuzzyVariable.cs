using Aquarium.AI.Fuzzy_Logic.Fuzzy_Sets;
using Aquarium.AI.Fuzzy_Logic.Fuzzy_Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI.Fuzzy_Logic
{
    public class FuzzyVariable
    {
        public string Name;
        public Dictionary<string, FuzzyTermSet> MemberSets = new Dictionary<string, FuzzyTermSet>();
        public double MinRange = 0.0;
        public double MaxRange = 0.0;

        public FuzzyVariable(string name)
        {
            Name = name;
        }

        public void AdjustRangeToFit(double min, double max)
        {
            MinRange = min;
            MaxRange = max;
        }

        public FuzzyTermSet AddLeftShoulderSet(string name, double min, double peak, double max)
        {
            FuzzyTermSet set = new FuzzyTermSet(name);
            set.Set = new LeftShoulderSet(min, peak, max);
            MemberSets.Add(name, set);
            return set;
        }
        public FuzzyTermSet AddRightShoulderSet(string name, double min, double peak, double max)
        {
            FuzzyTermSet set = new FuzzyTermSet(name);
            set.Set = new RightShoulderSet(min, peak, max);
            MemberSets.Add(name, set);
            return set;
        }
        public FuzzyTermSet AddTriangularSet(string name, double min, double peak, double max)
        {
            FuzzyTermSet set = new FuzzyTermSet(name);
            set.Set = new TriangularSet(min, peak, max);
            MemberSets.Add(name, set);
            return set;
        }

        public void Fuzzify(ref double val)
        {
            foreach (KeyValuePair<string, FuzzyTermSet> memberSet in MemberSets)
            {
                memberSet.Value.Set.DOM = memberSet.Value.Set.CalculateDOM(val);
            }
        }
        public double Defuzzify()
        {
            return 0.0;
        }
        public double DefuzzifyCentroid(int samples)
        {
            return 0.0;
        }

        public override string ToString()
        {
            string output = $"Fuzzy variable information for {Name}:\n";
            foreach (KeyValuePair<string, FuzzyTermSet> memberSet in MemberSets)
            {
                output += "-- Member set" + '\n';
                output += "-- Type: " + memberSet.Value.Set.GetType() + '\n';
                output += "-- Key: " + memberSet.Key + '\n';
                output += "-- Value of set DOM: " + memberSet.Value.Set.DOM + '\n';
                output += "-- Value of set representative: " + memberSet.Value.Set.Represents + "\n\n";
            }
            return output;
        }
    }
}
