using System;
using System.Collections.Generic;

// ========================
// Revision 2020.10.03
// ========================

namespace TheGame
{
    public enum FigureBlockColorKeys
    {
        White = 0,
        Red = 1,
        Orange = 2,
        Yellow = 3,
        Green = 4,
        Cyan = 5,
        Blue = 6,
        Violet = 7
    }

    public enum FigureTypeKeys
    {
        None = 0,
        O = 1,
        I = 2,
        S = 3,
        Z = 4,
        L = 5,
        J = 6,
        T = 7
    }


    public static class Enums
    {
        public static void RecreateList<T>(ref List<T> list, Type enumType)
        {
            var values = Enum.GetValues(enumType);
            var maxIndex = (int)values.GetValue(values.Length - 1);
            list = new List<T>(maxIndex);

            for (int i = 0; i <= maxIndex; i++)
                list.Add(default);
        }
    }
}