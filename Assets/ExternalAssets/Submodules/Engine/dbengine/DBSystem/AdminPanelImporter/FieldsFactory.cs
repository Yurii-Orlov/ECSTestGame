using System.Collections.Generic;

namespace Core.Database
{
	public static class FieldsFactory
	{
		private static readonly Dictionary<string, Dictionary<string, int>> FieldsDic =
			new Dictionary<string, Dictionary<string, int>>
			{
				{
					"ShipData", new Dictionary<string, int>
					{
						{"IdShipData", 		0},
						{"IdShipType", 		1},
						{"ShipBasicHp",		2},
						{"ShipSpeed", 		3},
						{"Compartments", 	4},
						{"Length", 			5},
					}
				},
				{
					"ShipName", new Dictionary<string, int>
					{
						{"IdShipName", 		0},
						{"IdShipData", 		1},
						{"IdShipNation",	2},
						{"Name", 			3},
						{"AlternativeName", 4},
					}
				},
				{
					"ArmorData", new Dictionary<string, int>
					{
						{"IdArmorData", 	    0},
						{"IdShipData", 		    1},
						{"IdArmorPartLocation",	2},
						{"IdArmorValue",        3},
					}
				}
			};

		public static int GetFieldIndex(string className, string fieldName)
		{
			if (!FieldsDic.ContainsKey(className)) return ErrorCodes.IntError;
			
			var classDict = FieldsDic[className];
			return classDict.ContainsKey(fieldName) ? classDict[fieldName] : ErrorCodes.IntError;
		}
	}
}