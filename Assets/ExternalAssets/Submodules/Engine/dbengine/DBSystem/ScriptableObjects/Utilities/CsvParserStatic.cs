using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvParser
{
	public class CsvParserStatic 
	{
		public static IEnumerable<string> GetLines(string csvText)
		{
			var lines = csvText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			return lines.AsEnumerable();
		}

		public static IEnumerable<IEnumerable<string>> GetAllRecordsSkipHeaders(string csvText)
		{
			var lines = GetLines(csvText);

			return lines.Skip(1).Select(CsvParser);
		}

        public static IEnumerable<IEnumerable<string>> GetAllRecordsWithoutSkipHeaders(string csvText)
        {
            var lines = GetLines(csvText);

            return lines.Select(CsvParser);
        }

        public static IEnumerable<string> CsvParser(string csvText)
		{
			var tokens = new List<string>();

			var last = -1;
			var current = 0;
			var inText = false;

			while (current < csvText.Length)
			{
				switch (csvText[current])
				{
					case '"':
						inText = !inText;
						break;
					case ',':
						if (!inText)
						{
							var tokenStr = csvText.Substring(last + 1, current - last).Trim(' ', ',');

							tokenStr = GetConvertedStr(tokenStr);

							//DebugLogger.Log(this, "tokenStr: " + tokenStr);
							tokens.Add(tokenStr);
							last = current;
						}
						break;
				}
				current++;
			}

			if (last != csvText.Length - 1)
			{
				var tokenStr = csvText.Substring(last + 1).Trim();

				tokenStr = GetConvertedStr(tokenStr);
				
				tokens.Add(tokenStr);
				
			}

			return tokens;
		}

		private static string GetConvertedStr(string tokenStr)
		{
			if (tokenStr.StartsWith("\"") && tokenStr.EndsWith("\""))
			{
				tokenStr = tokenStr.Substring(1, tokenStr.Length - 2);
			}
				
			tokenStr = tokenStr.Replace("\"\"", "\"");
			return tokenStr;
		}
	}
	
}
