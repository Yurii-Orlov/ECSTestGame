using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomLogger;
using UnityEngine;

namespace Core.Database
{
	public class ParseDataScheme
	{
		private static readonly string[] TableFilteredNames =
		{
			"ShipData", "ShipName"
		};

        private static int a = 0;

		//splitted by ','
		public static List<string> ParseScheme(string jsonStr)
		{
			var resultStrings = new List<string>();
			int? firstIndex = null;
			var scopeLevel = 0;
			for (var i = 0; i < jsonStr.Length; i++)
			{
				if (jsonStr[i] == ',' && scopeLevel == 0)
				{
					resultStrings.Add(jsonStr.Substring(firstIndex.GetValueOrDefault(), i - firstIndex.GetValueOrDefault()));
					firstIndex = i + 1;
				}
				else
					switch (jsonStr[i])
					{
						case '{':
							scopeLevel++;
							break;
						case '}':
							scopeLevel--;
							break;
						case '[':
							scopeLevel++;
							break;
						case ']':
							scopeLevel--;
							break;
					}
			}
			resultStrings.Add(jsonStr.Substring(firstIndex.GetValueOrDefault()));
			return resultStrings;
		}
		
		public static IEnumerable<SchemeRecord> ParseSchemeTest(string jsonStr)
		{
			var tablesJsons = ParseScheme(jsonStr);
			var scriptsData = tablesJsons.Select(ScriptStringGenerator.ParseSchemeRecord).ToArray();

			var filteredData = scriptsData.Where(x => TableFilteredNames.Contains(x.ClassName));

			return filteredData;
		}
	}
}