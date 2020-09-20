using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Utils;
using CustomLogger;
using UnityEngine;

namespace Core.Database
{
	public static class ParseDataData
	{
		private static readonly string[] TableFilteredNames =
		{
			"ShipData", "ShipName",
			
			"ShipEquipment", "ShipAaBattery", "ShipTorpedoBattery", "ShipBbBattery", "AswDefaultEquipment",
            "EquipmentData", "TorpedoProjectileData", "RefitSlotUnlockThresholdData",

            "NavalRifleData", "NavalRifleBallisticData", "ComputerData", "DirectorData", "RadarData",
			"Asdic", "QuarterdeckData", "ArmorPart", "ArmorData",
			"InstructionData", "DivisionData", "DivisionList", "FleetTacticsData",
			"ArmorPart", "ArmorData", "WeatherData", "SeaStateData",
			
			"CriticalHit",
            "CriticalHitEffect",
			"VesturesData",
			"AntiAirGun",
			"AircraftData",
			"AircraftOpportunityComponent",
			"DiveBomberComponent",
			"DiveBomberProjectile",
			"TorpedoBomberComponent",
            "FighterComponent",
            "ScoutComponent",
            "AircraftLaunchSystem",
            "AircraftSquadron",
			"AircraftStorage",
			"SquadronType",
            "EquipImageData",
            "CombatImageData",
        };
		
		public static IEnumerable<TableRecordsClass> ParseData(string jsonStr)
		{
			//DebugLogger.Log(null, "jsonStr: " + jsonStr);
			
			var tablesStr = GetTablesSrings(jsonStr);
			var tablesData = tablesStr.Select(GetTablesDataClass);
			var tablesWithRecords = tablesData.Select(GetFields);
			var filtered = tablesWithRecords.Where(x => TableFilteredNames.Contains(x.ClassName));
			return filtered;
		}

		private static IEnumerable<string> GetTablesSrings(string jsonStr)
		{
			return ParseDataScheme.ParseScheme(jsonStr);
		}

		private static TableDataClass GetTablesDataClass(string jsonStr)
		{
			var tableJsonName = jsonStr.Split('"', '"')[1];
			var finalTableName = ScriptStringGenerator.UppercaseWords(tableJsonName);
			
			var tablenameBeforeBrackets = jsonStr.Split('[', ']')[0];
			//+1 - because we need delete bracket
			var innerData = jsonStr.Remove(0, tablenameBeforeBrackets.Length + 1);
			innerData = innerData.Remove(innerData.Length - 1);
			
			//DebugLogger.Log(null, "GetTablesDataClass: " + tableJsonName);
			return new TableDataClass
			{
				ClassName = finalTableName,
				ClassData = innerData
			};
		}

		private static TableRecordsClass GetFields(TableDataClass data)
		{
			var innerDataList = ParseDataScheme.ParseScheme(data.ClassData).ToList();
            
            List<RecordData> records = new List<RecordData>();
            for(int i = 0; i < innerDataList.Count; i++)
            {
                string str = innerDataList[i];
                RecordData record = CreateRecord(str);
                if (record != null)
                {
                    records.Add(record);
                }
            }
            
			var tableRecordsClass = new TableRecordsClass
			{
				ClassName = data.ClassName,
				ClassData = records
			};
			return tableRecordsClass;
		}

        private static RecordData CreateRecord(string json)
        {
            if (json.Length <= 0) return null;

            json = json.Remove(0, 1);
            json = json.Remove(json.Length - 1, 1);

            bool stringData = false;
            int lastSplitter = 0;
            List<string> values = new List<string>();
            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"'  && (i == 0 || json[i - 1] == ',')) stringData = true;
                if (c == '"' && (i == json.Length - 1 || json[i + 1] == ',')) stringData = false;

                if (c == ',' && !stringData)
                {
                    values.Add(json.Substring(lastSplitter, i - lastSplitter));
                    lastSplitter = i + 1;
                }

                if (i == json.Length - 1)
                {
                    values.Add(json.Substring(lastSplitter, json.Length - lastSplitter));
                }
            }
            
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = values[i].Replace(",", "").Replace("\"", "").Replace("\\", "");
            }

            RecordData rec = new RecordData()
            {
                DataList = values.ToList()
            };

            return rec;
        }

		//private static RecordData CreateRecord(string json)
		//{
		//	if (json.Length <= 0) return null;

            
		//	//Debug.Log(json.Length);
		//	//remove ',' in names like : Type Croiseur de 10,000-tonnes
		//	var converted = new Regex("\"(.*?)\"", RegexOptions.Singleline);
		//	var matches = converted.Matches(json);
		//	foreach (Match o in matches)
		//	{
		//		var removed = o.Value.Replace(",", "");
		//		json = json.Replace(o.Value, removed);
		//	}
			
		//	//Debug.Log(json.Length);
		//	var jsonCleaned = json.Remove(0, 1);
		//	jsonCleaned = jsonCleaned.Remove(jsonCleaned.Length - 1);
		//	jsonCleaned = jsonCleaned.Replace("\"", "");
		//	jsonCleaned = jsonCleaned.Replace("\\", "");
		//	var fieldsValues = jsonCleaned.Split(',');
  //          if (fieldsValues[0] == "561")
  //          {
  //              Debug.Log("aaa");
  //          }
		//	var recordData = new RecordData
		//	{
		//		DataList = fieldsValues.ToList()
		//	};

		//	return recordData;
		//}
	}

	public class TableDataClass
	{
		public string ClassName;
		public string ClassData;
	}

	public class TableRecordsClass
	{
		public string ClassName;
		public List<RecordData> ClassData;

		public override string ToString()
		{
			var str = ClassData.Aggregate(ClassName + ":\n", (current, data) => current + data + "\n");
			return str;
		}
	}

	public class RecordData
	{
		public List<string> DataList;
		
		public override string ToString()
		{
			var str = DataList.Aggregate("", (current, data) => current + data + " ");
			return str;
		}
	}
}