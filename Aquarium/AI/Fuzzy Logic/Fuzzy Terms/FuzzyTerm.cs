using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI.Fuzzy_Logic.Fuzzy_Terms
{
    public interface IFuzzyTerm
    {
        IFuzzyTerm Clone();
        double GetDOM();
        void ClearDOM();
        void ORwithDOM(double val);
    }
}
