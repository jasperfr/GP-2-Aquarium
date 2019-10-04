using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI.Fuzzy_Logic.Fuzzy_Sets
{
    public class RightShoulderSet : FuzzySet
    {
        public double Left, Peak, Right;

        public RightShoulderSet(double left, double peak, double right)
            : base((peak + right) / 2)
        {
            Left = left;
            Peak = peak;
            Right = right;
        }

        public override double CalculateDOM(double val)
        {
            // At the center
            if (Equals(Peak, val))
            {
                return 1.0;
            }

            // To the left of the center
            if ((val < Peak) && (val >= Left))
            {
                double offset = val - Left;
                double deltaY = 1.0 / (Peak - Left);
                double sum = offset * deltaY;
                return sum;
            }

            // To the right of the center
            else if (val > Peak)
            {
                return 1.0;
            }

            // Else out of range, return 0
            else return 0.0;
        }
        public override string GetSetName()
        {
            return "Right";
        }
    }
}
