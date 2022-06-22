using System;

namespace MyLab.WebErrors
{
    static class TypeComparer
    {
        public static int GetComparisonRate(Type baseType, Type targetType)
        {
            if (baseType == null || targetType == null)
                return -1;

            if (baseType == targetType)
                return 0;

            if (targetType.BaseType == null)
                return -1;

            int rate = 0;

            Type seed = targetType;

            do
            {
                seed = seed.BaseType;
                rate++;

                if (baseType == seed)
                    return rate;

            } while (seed?.BaseType != null);

            return -1;
        }
    }
}