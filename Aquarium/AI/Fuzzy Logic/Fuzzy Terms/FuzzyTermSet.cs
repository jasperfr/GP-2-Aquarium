using Aquarium.AI.Fuzzy_Logic.Fuzzy_Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI.Fuzzy_Logic.Fuzzy_Terms
{
    /* 
     * I don't know either, the book is confusing. 
     * Represents a Fuzzy Term atomic object.
     */
    public class FuzzyTermSet : IFuzzyTerm
    {
        public string VariableName;
        public FuzzySet Set;

        public FuzzyTermSet(string name)
        {
            VariableName = name;
        }

        public void ClearDOM() => Set.Clear();
        public IFuzzyTerm Clone()
        {
            FuzzySet cloned = Set.Clone();
            FuzzyTermSet clone = (FuzzyTermSet)this.MemberwiseClone();
            clone.Set = cloned;
            return clone;
        }
        public double GetDOM() => Set.Get();
        public void ORwithDOM(double val) => Set.Or(val);

        public override string ToString()
        {
            return $"[{VariableName}] {Set.ToString()}";
        }
    }
}
