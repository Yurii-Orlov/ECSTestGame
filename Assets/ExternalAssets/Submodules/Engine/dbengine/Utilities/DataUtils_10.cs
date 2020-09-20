using System;
using System.Collections.Generic;

namespace Core.Utils
{
	// ReSharper disable once InconsistentNaming
	public static class DataUtils_10 
	{
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach(var item in enumeration)
			{
				action(item);
			}
		}
		
		public static T[] Add<T>(this T[] target, params T[] items)
		{
			// Validate the parameters
			if (target == null) {
				target = new T[] { };
			}
			if (items== null) {
				items = new T[] { };
			}

			// Join the arrays
			var result = new T[target.Length + items.Length];
			target.CopyTo(result, 0);
			items.CopyTo(result, target.Length);
			return result;
		}
		
		public static IEnumerable<T> WithoutLast<T>(this IEnumerable<T> source)
		{
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext()) yield break;
				
				for (var value = e.Current; e.MoveNext(); value = e.Current)
				{
					yield return value;
				}
			}
		}
	}

	public static class RomanNumberConvertor
	{
		public static string Convert(int number)
		{
			switch (number)
			{
				case 1:
					return "I";
				case 2:
					return "II";
				case 3:
					return "III";
				case 4:
					return "IV";
				case 5:
					return "V";
				case 6:
					return "VI";
				default:
					return null;
			}
		}
	}

	
	
	

}
