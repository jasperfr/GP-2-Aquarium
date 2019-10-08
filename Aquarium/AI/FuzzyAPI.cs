using Aquarium.AI.Fuzzy_Logic;
using Aquarium.AI.Fuzzy_Logic.Fuzzy_Terms;
using System;

namespace Aquarium.AI
{
    /*
     * This static class is mainly used to create, modify and use Fuzzy Logic sets inside your Lua file.
     */
    public static class FuzzyAPI
    {
        public enum Sets
        {
            LEFT_SHOULDER,
            TRIANGULAR,
            RIGHT_SHOULDER
        }

        public static FuzzyModule create_fuzzy_module() => new FuzzyModule();
        public static FuzzyVariable create_flv(FuzzyModule module, string name) => module.CreateFLV(name);
        public static FuzzyTermSet add_term(Sets set, FuzzyVariable fuzzyVar, string name, double min, double peak, double max)
        {
            switch(set)
            {
                case Sets.LEFT_SHOULDER:
                    return fuzzyVar.AddLeftShoulderSet(name, min, peak, max);
                case Sets.TRIANGULAR:
                    return fuzzyVar.AddTriangularSet(name, min, peak, max);
                case Sets.RIGHT_SHOULDER:
                    return fuzzyVar.AddRightShoulderSet(name, min, peak, max);
                default:
                    throw new ArgumentException("Cannot find the desired fuzzy set.");
            }
        }
        public static void add_rule(FuzzyModule module, FuzzyTermSet IF, FuzzyTermSet AND, FuzzyTermSet THEN) => module.AddRule(new FuzzyAND(IF, AND), THEN);
        /* Right now the calculate function only relies on 2 input sets and 1 output set... */
        public static double calculate(FuzzyModule module, string xname, double xval, string yname, double yval, string outname)
        {
            module.Fuzzify(xname, ref xval);
            module.Fuzzify(yname, ref yval);
            return module.Defuzzify(outname, FuzzyModule.DefuzzifyType.CENTROID);
        }
    }
}
