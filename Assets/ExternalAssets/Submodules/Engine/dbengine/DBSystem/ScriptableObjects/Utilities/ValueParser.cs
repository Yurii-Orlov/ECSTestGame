using System.Linq;

namespace Core.Data
{
    public static class ValueParser
    {
        private static readonly string[] CharsToRemove =
            {"c", "r", "-", "'", "*", "V", "H", "\"", "N", "\\", "/", "A", "u", "s", "h"};

        public static float GetFloatValueFromString(string stringValue)
        {
            stringValue = CharsToRemove.Aggregate(stringValue, (current, c) => current.Replace(c, string.Empty));
            //DebugLogger.Log(null, "stringValue: " + stringValue, LogColor.Aquamarine);
            var floatValue = 0f;
            if (stringValue != "")
            {
                floatValue = float.Parse(stringValue.Replace(",", "."));
            }
            return floatValue;
        }

        public static int GetIntValueFromString(string stringValue)
        {
            stringValue = CharsToRemove.Aggregate(stringValue, (current, c) => current.Replace(c, string.Empty));
            var intValue = 0;
            if (stringValue != "")
            {
                intValue = int.Parse(stringValue);
            }
            return intValue;
        }
    }
}
