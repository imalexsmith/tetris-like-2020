using System;
using UnityEngine;

// ========================
// Revision 2020.10.16
// ========================

namespace NightFramework
{
    public enum PropertyEffectTypes
    {
        [InspectorName("Add (+, or)")]
        Add = 0,
        [InspectorName("Multiply (*, and)")]
        Multiply = 1
    }

    public static class PropertyEffectExtentions
    {
        public static void ApplyEffect(this ref int target, PropertyEffectInt effect)
        {
            target = effect.Apply(target);
        }

        public static void UnapplyEffect(this ref int target, PropertyEffectInt effect)
        {
            target = effect.Unapply(target);
        }

        public static void ApplyEffect(this ref float target, PropertyEffectFloat effect)
        {
            target = effect.Apply(target);
        }

        public static void UnapplyEffect(this ref float target, PropertyEffectFloat effect)
        {
            target = effect.Unapply(target);
        }

        public static void ApplyEffect(this ref bool target, PropertyEffectBool effect)
        {
            target = effect.Apply(target);
        }

        public static void UnapplyEffect(this ref bool target, PropertyEffectBool effect)
        {
            target = effect.Unapply(target);
        }
    }

    public abstract class PropertyEffect<T>
    {
        public bool Applicable;
        public PropertyEffectTypes EffectType;
        public T Value;

        public abstract T Apply(T initialValue);

        public abstract T Unapply(T initialValue);
    }

    [Serializable]
    public class PropertyEffectInt : PropertyEffect<int>
    {
        public override int Apply(int initialValue)
        {
            if (!Applicable)
                return initialValue;

            switch (EffectType)
            {
                case PropertyEffectTypes.Add:
                    initialValue += Value;
                    break;
                case PropertyEffectTypes.Multiply:
                    initialValue *= Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return initialValue;
        }

        public override int Unapply(int initialValue)
        {
            if (!Applicable)
                return initialValue;

            switch (EffectType)
            {
                case PropertyEffectTypes.Add:
                    initialValue -= Value;
                    break;
                case PropertyEffectTypes.Multiply:
                    initialValue /= Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return initialValue;
        }
    }

    [Serializable]
    public class PropertyEffectFloat : PropertyEffect<float>
    {
        public override float Apply(float initialValue)
        {
            if (!Applicable)
                return initialValue;

            switch (EffectType)
            {
                case PropertyEffectTypes.Add:
                    initialValue += Value;
                    break;
                case PropertyEffectTypes.Multiply:
                    initialValue *= Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return initialValue;
        }

        public override float Unapply(float initialValue)
        {
            if (!Applicable)
                return initialValue;

            switch (EffectType)
            {
                case PropertyEffectTypes.Add:
                    initialValue -= Value;
                    break;
                case PropertyEffectTypes.Multiply:
                    initialValue /= Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return initialValue;
        }
    }

    [Serializable]
    public class PropertyEffectBool : PropertyEffect<bool>
    {
        public override bool Apply(bool initialValue)
        {
            if (!Applicable)
                return initialValue;

            switch (EffectType)
            {
                case PropertyEffectTypes.Add:
                    initialValue = initialValue | Value;
                    break;
                case PropertyEffectTypes.Multiply:
                    initialValue = initialValue & Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return initialValue;
        }

        public override bool Unapply(bool initialValue)
        {
            if (!Applicable)
                return initialValue;

            switch (EffectType)
            {
                case PropertyEffectTypes.Add:
                    initialValue = initialValue & !Value;
                    break;
                case PropertyEffectTypes.Multiply:
                    initialValue = initialValue | !Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return initialValue;
        }
    }
}