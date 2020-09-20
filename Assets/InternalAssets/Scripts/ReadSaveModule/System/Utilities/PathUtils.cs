
using System.IO;


namespace InternalAssets.Scripts.Engine.Utilities
{
    public static class PathUtils
    {
        public static string Combine(string part1, string part2)
        {
            //#if UNITY_IOS || UINTY_OSX
            //			return string.Format(@"{0}\{1}", part1, part2);
            //#endif
            //return string.Format("{0}/{1}", part1, part2);
            return Path.Combine(part1, part2);
        }
    }
}
