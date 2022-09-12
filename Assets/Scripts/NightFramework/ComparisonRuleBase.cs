using System;

// ========================
// Revision 2019.11.15
// ========================

namespace NightFramework
{
    public abstract class ComparisonRuleBase
    {
        public enum ComparisonCondition
        {
            Equals = 0,
            NotEqual = 1,
            Less = 2,
            LessOrEqual = 3,
            More = 4,
            MoreOrEqual = 5
        }


        // ==============================================================================================
        public ComparisonCondition Condition;

        public abstract bool Evaluate();
    }


    public abstract class ComparisonRuleFloat : ComparisonRuleBase
    {
        // ==============================================================================================
        public float Value;

        protected bool Evaluate(float reference)
        {
            switch (Condition)
            {
                case ComparisonCondition.Equals:
                    return reference.Equals(Value);
                case ComparisonCondition.NotEqual:
                    return !reference.Equals(Value);
                case ComparisonCondition.Less:
                    return reference < Value;
                case ComparisonCondition.LessOrEqual:
                    return reference <= Value;
                case ComparisonCondition.More:
                    return reference > Value;
                case ComparisonCondition.MoreOrEqual:
                    return reference >= Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    public abstract class ComparisonRuleInt : ComparisonRuleBase
    {
        // ==============================================================================================
        public int Value;

        protected bool Evaluate(int reference)
        {
            switch (Condition)
            {
                case ComparisonCondition.Equals:
                    return reference.Equals(Value);
                case ComparisonCondition.NotEqual:
                    return !reference.Equals(Value);
                case ComparisonCondition.Less:
                    return reference < Value;
                case ComparisonCondition.LessOrEqual:
                    return reference <= Value;
                case ComparisonCondition.More:
                    return reference > Value;
                case ComparisonCondition.MoreOrEqual:
                    return reference >= Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    public abstract class ComparisonRuleBool : ComparisonRuleBase
    {
        // ==============================================================================================
        public bool Value;

        protected bool Evaluate(bool reference)
        {
            switch (Condition)
            {
                case ComparisonCondition.Equals:
                    return reference.Equals(Value);
                case ComparisonCondition.NotEqual:
                    return !reference.Equals(Value);
                case ComparisonCondition.Less:
                case ComparisonCondition.LessOrEqual:
                case ComparisonCondition.More:
                case ComparisonCondition.MoreOrEqual:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
