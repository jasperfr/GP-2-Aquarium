using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI.Fuzzy_Logic.Fuzzy_Terms
{
    public class FuzzyAND : IFuzzyTerm
    {
        public List<IFuzzyTerm> Terms = new List<IFuzzyTerm>();

        public FuzzyAND(IFuzzyTerm term1, IFuzzyTerm term2)
        {
            Terms.Add(term1);
            Terms.Add(term2);
        }
        public FuzzyAND(IFuzzyTerm term1, IFuzzyTerm term2, IFuzzyTerm term3) : this(term1, term2)
        {
            Terms.Add(term3);
        }
        public FuzzyAND(IFuzzyTerm term1, IFuzzyTerm term2, IFuzzyTerm term3, IFuzzyTerm term4) : this(term1, term2, term3)
        {
            Terms.Add(term4);
        }

        public void ClearDOM() => Terms.ForEach(t => t.ClearDOM());
        public IFuzzyTerm Clone()
        {
            FuzzyAND clone = (FuzzyAND)this.MemberwiseClone();
            clone.Terms = new List<IFuzzyTerm>();
            Terms.ForEach(term => clone.Terms.Add(term.Clone()));
            return clone;
        }

        public double GetDOM()
        {
            double minDOM = Double.MaxValue;
            foreach (IFuzzyTerm term in Terms)
            {
                if (term.GetDOM() < minDOM)
                {
                    minDOM = term.GetDOM();
                }
            }
            return minDOM;
        }

        public void ORwithDOM(double val) => Terms.ForEach(t => t.ORwithDOM(val));

        public override string ToString()
        {
            return "(" + Terms[0].ToString() + " AND " + Terms[1].ToString() + ")";
        }
    }
}
