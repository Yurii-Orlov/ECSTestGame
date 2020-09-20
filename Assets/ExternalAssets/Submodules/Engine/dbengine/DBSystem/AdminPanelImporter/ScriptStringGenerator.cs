using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Database
{
	public static class ScriptStringGenerator
	{
		private static readonly string[] TableTypes = {"Record", "Type"};
		
		public static string UppercaseWords(string value)
		{
			var array = value.ToCharArray();
			// Handle the first letter in the string.
			if (array.Length >= 1)
			{
				if (char.IsLower(array[0]))
				{
					array[0] = char.ToUpper(array[0]);
				}
			}

			// Scan through the letters, checking for spaces.
			// ... Uppercase the lowercase letters following spaces.
			for (var i = 1; i < array.Length; i++)
			{
				if (array[i - 1] != '_') continue;

				if (char.IsLower(array[i]))
				{
					array[i] = char.ToUpper(array[i]);
				}
			}

			var arrayWithoutUnderscores = array.Where(x => x != '_').ToArray();

			return new string(arrayWithoutUnderscores);
		}

		private static string GetClearedTableName(string fullName)
		{
			var result = string.Empty;

			foreach (var item in TableTypes)
			{
				if (!fullName.EndsWith(item)) continue;
				
				result = fullName.Substring(0, fullName.LastIndexOf(item, StringComparison.Ordinal));
				break; //only allow one match at most
			}	

			return result;
		}
		
		private static IEnumerable<string> ParseFields(IEnumerable<string> fieldsStrokes)
		{
			var result = from stroke in fieldsStrokes
				select stroke.Split(':')
				into twoValues
				select twoValues[0].Replace("\"", "")
				into leftValue
				select UppercaseWords(leftValue);
			return result;
		}

		public static SchemeRecord ParseSchemeRecord(string jsonStr)
		{
			var tableJsonName = jsonStr.Split('"', '"')[1];
			var finalTableName = UppercaseWords(tableJsonName);
			var clearedTableName = GetClearedTableName(finalTableName);

			var innerData = jsonStr.Split('{', '}')[1];
			var innerArray = innerData.Split(',');
			var fields = ParseFields(innerArray);

			var scheme = new SchemeRecord
			{
				ClassName = clearedTableName,
				FieldNames = fields.ToArray()
			};
			return scheme;
		}
	}

	public class SchemeRecord
	{
		public string ClassName;
		public string[] FieldNames;

		public override string ToString()
		{
			var result = ClassName + ": ";
			var fields = FieldNames.Aggregate("", (s, s1) => s += s1 + " ");

			result += fields;
			return result;
		}
	}
}