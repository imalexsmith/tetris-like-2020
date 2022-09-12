using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

// ========================
// Revision 2020.10.13
// ========================

namespace NightFramework
{
    public enum RandomValueSelectionMode
    {
        Single = 0,
        Every = 1,
        [InspectorName("With Repeats")]
        SeveralWithRepeats = 2,
        [InspectorName("With No Repeats")]
        SeveralWithNoRepeats = 3
    }

    [Serializable]
    public struct RandomisedSetEntry<T>
    {
        public T Value;
        public bool Always;
        [ConditionalField(nameof(Always), true), Min(0f)]
        public float Weight;
    }

    [Serializable]
    public class RandomisedSet<T>
    {
        // ========================================================================================
        public RandomValueSelectionMode SelectionMode;
        [Min(1)]
        public int SelectionRounds;
        public List<RandomisedSetEntry<T>> Values = new List<RandomisedSetEntry<T>>();


        // ========================================================================================
        public List<T> SelectRandomValues()
        {
            var result = new List<T>();

            if (SelectionMode == RandomValueSelectionMode.Every)
            {
                result.AddRange(Values.Select(x => x.Value).Where(x => x != null));
            }
            else
            {
                var selectFrom = new List<RandomisedSetEntry<T>>();
                foreach (var value in Values)
                {
                    if (value.Always)
                        result.Add(value.Value);
                    else
                        selectFrom.Add(value);
                }

                if (selectFrom.Count == 0)
                    return result;

                int rounds;
                switch (SelectionMode)
                {
                    default:
                        throw new UnityException(Exceptions.UnexpectedBehaviourWhileRandomSelection);
                    case RandomValueSelectionMode.Single:
                        rounds = 1;
                        break;
                    case RandomValueSelectionMode.SeveralWithRepeats:
                        rounds = SelectionRounds;
                        break;
                    case RandomValueSelectionMode.SeveralWithNoRepeats:
                        rounds = Mathf.Min(SelectionRounds, selectFrom.Count);
                        break;
                }

                for (int i = 0; i < rounds; i++)
                {
                    var single = SelectSingle(selectFrom);
                    
                    if (single.Value != null)
                        result.Add(single.Value);

                    if (SelectionMode == RandomValueSelectionMode.SeveralWithNoRepeats)
                        selectFrom.Remove(single);
                }
            }

            return result;
        }

        private RandomisedSetEntry<T> SelectSingle(IEnumerable<RandomisedSetEntry<T>> from)
        {
            var sumWeight = from.Sum(x => x.Weight);
            var r = UnityEngine.Random.Range(0, sumWeight);
            foreach (var item in from)
            {
                r -= item.Weight;
                if (r <= 0)
                    return item;
            }

            throw new UnityException(Exceptions.UnexpectedBehaviourWhileRandomSelection);
        }
    }
}