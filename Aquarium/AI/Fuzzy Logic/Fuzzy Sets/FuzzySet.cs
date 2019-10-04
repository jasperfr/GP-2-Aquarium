using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI.Fuzzy_Logic.Fuzzy_Sets
{
    public abstract class FuzzySet
    {
        public double DOM;
        public double Represents;

        public FuzzySet Clone() => (FuzzySet)MemberwiseClone();

        public FuzzySet(double represents)
        {
            DOM = 0.0;
            Represents = represents;
        }

        public abstract double CalculateDOM(double value);
        public abstract string GetSetName();

        public void Or(double value) => DOM = Math.Max(DOM, value);
        public double Get() => DOM;
        public void Clear() => DOM = 0.0;

        public override string ToString()
        {
            return GetSetName() + "[" + Get() + "]" ;
        }

    }
}
