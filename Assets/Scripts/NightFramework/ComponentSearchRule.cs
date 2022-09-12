using System;
using UnityEngine;


namespace NightFramework
{
    [Serializable]
    public class ComponentSearchRule
    {
        // ==============================================================================================
        public enum SearchType
        {
            ByObjectName,
            ByObjectTag,
            ByClassName
        }

        public enum SearchCondition
        {
            Equals,
            StartsWith,
            EndsWith,
            Contains
        }


        // ==============================================================================================
        public SearchType Type;
        public SearchCondition Condition;
        public string Value;


        // ==============================================================================================
        public ComponentSearchRule(string value = "", SearchCondition condition = SearchCondition.Equals, SearchType type = SearchType.ByObjectName)
        {
            Value = value;
            Type = type;
            Condition = condition;
        }

        public bool Evaluate(Component comp)
        {
            if (comp == null)
                return false;

            if (string.IsNullOrWhiteSpace(Value))
                return true;

            string reference;
            switch (Type)
            {
                case SearchType.ByObjectName:
                    reference = comp.name;
                    break;
                case SearchType.ByObjectTag:
                    reference = comp.tag;
                    break;
                case SearchType.ByClassName:
                    reference = comp.GetType().Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (Condition)
            {
                case SearchCondition.Equals:
                    return reference.Equals(Value, StringComparison.InvariantCultureIgnoreCase);
                case SearchCondition.StartsWith:
                    return reference.StartsWith(Value, StringComparison.InvariantCultureIgnoreCase);
                case SearchCondition.EndsWith:
                    return reference.EndsWith(Value, StringComparison.InvariantCultureIgnoreCase);
                case SearchCondition.Contains:
                    return reference.IndexOf(Value, StringComparison.InvariantCultureIgnoreCase) > -1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
