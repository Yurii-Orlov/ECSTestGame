using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomLogger;
using UnityEngine;

namespace Core.Database
{
	public static class TablesRecordsFiller
	{
		private static readonly Dictionary<string, string> PathDictionary = new Dictionary<string, string>
		{
			{"ShipData", "ScriptableObjects\\Core\\EntityDb\\ShipData\\ShipData"},
			{"ShipName", "ScriptableObjects\\Core\\EntityDb\\ShipData\\ShipName"},
			{"ShipEquipment", "ScriptableObjects\\Core\\EntityDb\\ShipData\\ShipEquipment"},

			{"ShipBbBattery", "ScriptableObjects\\Core\\EntityDb\\ShipData\\ShipBbBattery"},
			{"ShipTorpedoBattery", "ScriptableObjects\\Core\\EntityDb\\ShipData\\ShipTorpedoBattery"},
            {"ShipAaBattery", "ScriptableObjects\\Core\\EntityDb\\ShipData\\ShipAaBattery"},
            {"AswDefaultEquipment", "ScriptableObjects\\Core\\EntityDb\\ShipData\\AswDefaultEquipment"},
            {"RefitSlotUnlockThresholdData", "ScriptableObjects\\Core\\EntityDb\\ShipData\\RefitSlotUnlockThresholdData"},

            {"EquipmentData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\EquipmentData"},
			{"TorpedoProjectileData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\Torpedo\\TorpedoProjectileData"},
			{"NavalRifleData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\NavalRiffle\\NavalRifleData"},
            {"NavalRifleBallisticData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\NavalRiffle\\NavalRifleBallisticData"},
            {"ComputerData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\FCS\\ComputerData"},
			{"DirectorData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\FCS\\DirectorData"},
			{"Asdic", "ScriptableObjects\\Core\\EntityDb\\Equipments\\ASW\\Asdic"},
			{"QuarterdeckData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\ASW\\Quarterdeck"},
            {"VesturesData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\NonCombat\\VesturesData"},

            {"DivisionData", "ScriptableObjects\\Core\\EntityDb\\FleetTacticsData\\DivisionData"},
			{"DivisionList", "ScriptableObjects\\Core\\EntityDb\\FleetTacticsData\\DivisionList"},
			{"FleetTacticsData", "ScriptableObjects\\Core\\EntityDb\\FleetTacticsData\\FleetTacticsData"},
			{"InstructionData", "ScriptableObjects\\Core\\EntityDb\\FleetTacticsData\\InstructionData"},
			
			{"ArmorPart", "ScriptableObjects\\Core\\EntityDb\\ShipData\\Armor\\ArmorPart"},
			{"ArmorData", "ScriptableObjects\\Core\\EntityDb\\ShipData\\Armor\\ArmorData"},

			{"CriticalHit", "ScriptableObjects\\Core\\EntityDb\\CritEffects\\CriticalHit"},
			{"CriticalHitEffect", "ScriptableObjects\\Core\\EntityDb\\CritEffects\\CriticalHitEffect"},
			
			{"AircraftData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\AircraftData\\AircraftData"},
			{"AircraftOpportunityComponent", "ScriptableObjects\\Core\\EntityDb\\Equipments\\AircraftData\\AircraftOpportunityComponent"},
			{"DiveBomberComponent", "ScriptableObjects\\Core\\EntityDb\\Equipments\\AircraftData\\DiveBomberComponent"},
			{"DiveBomberProjectile", "ScriptableObjects\\Core\\EntityDb\\Equipments\\AircraftData\\DiveBomberProjectile"},
			{"TorpedoBomberComponent", "ScriptableObjects\\Core\\EntityDb\\Equipments\\AircraftData\\TorpedoBomberComponent"},
            {"FighterComponent", "ScriptableObjects\\Core\\EntityDb\\Equipments\\AircraftData\\FighterComponent"},
            {"ScoutComponent", "ScriptableObjects\\Core\\EntityDb\\Equipments\\AircraftData\\ScoutComponent"},

            {"AircraftLaunchSystem", "ScriptableObjects\\Core\\EntityDb\\ShipData\\Aircraft\\AircraftLaunchSystem"},
			{"AircraftSquadron", "ScriptableObjects\\Core\\EntityDb\\ShipData\\Aircraft\\AircraftSquadron"},
			{"AircraftStorage", "ScriptableObjects\\Core\\EntityDb\\ShipData\\Aircraft\\AircraftStorage"},
			{"SquadronType", "ScriptableObjects\\Core\\EntityDb\\ShipData\\Aircraft\\SquadronType"},
            {"WeatherData", "ScriptableObjects\\Core\\EntityDb\\Environment\\WeatherData"},
            {"SeaStateData", "ScriptableObjects\\Core\\EntityDb\\Environment\\SeaStateData"},
            {"EquipImageData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\EquipImageData"},
            {"CombatImageData", "ScriptableObjects\\Core\\EntityDb\\Equipments\\CombatImageData"}
        };

		private static readonly Dictionary<string, string> ScriptPathDictionary = new Dictionary<string, string>
		{
			{"ShipData", "Core.SimpleData.ShipData.ShipData"},
			{"ShipName", "Core.SimpleData.ShipData.ShipName"},
			{"ShipEquipment", "Core.SimpleData.ShipData.ShipEquipment"},

			{"ShipBbBattery", "Core.SimpleData.ShipData.ShipBbBattery"},
			{"ShipTorpedoBattery", "Core.SimpleData.ShipData.ShipTorpedoBattery"},
            {"ShipAaBattery", "Core.SimpleData.ShipData.ShipAaBattery"},
            {"AswDefaultEquipment", "Core.SimpleData.ShipData.AswDefaultEquipment"},
            {"RefitSlotUnlockThresholdData", "Core.SimpleData.ShipData.RefitSlotUnlockThresholdData"},
            {"EquipmentData", "Core.SimpleData.EquipmentData.EquipmentData"},
			{"TorpedoProjectileData", "Core.SimpleData.EquipmentData.TorpedoProjectileData"},
			{"NavalRifleData", "Core.SimpleData.EquipmentData.NavalRifleData"},
            {"NavalRifleBallisticData", "Core.SimpleData.EquipmentData.NavalRifleBallisticData"},
            {"ComputerData", "Core.SimpleData.EquipmentData.ComputerData"},
			{"DirectorData", "Core.SimpleData.EquipmentData.DirectorData"},
			{"Asdic", "Core.SimpleData.EquipmentData.Asdic"},
			{"QuarterdeckData", "Core.SimpleData.EquipmentData.Quarterdeck"},
            {"VesturesData", "Core.SimpleData.EquipmentData.VesturesData"},

            {"DivisionData", "Core.SimpleData.FleetTacticsData.DivisionData"},
			{"DivisionList", "Core.SimpleData.FleetTacticsData.DivisionList"},
			{"FleetTacticsData", "Core.SimpleData.FleetTacticsData.FleetTacticsData"},
			{"InstructionData", "Core.SimpleData.FleetTacticsData.InstructionData"},
			
			{"ArmorPart", "Core.SimpleData.ShipData.Armor.ArmorPart"},
			{"ArmorData", "Core.SimpleData.ShipData.Armor.ArmorData"},

			{"CriticalHit", "Core.SimpleData.Battle.CritEffects.CriticalHit"},
			{"CriticalHitEffect", "Core.SimpleData.Battle.CritEffects.CriticalHitEffect"},
			
			{"AircraftData", "Core.SimpleData.EquipmentData.AircraftData.AircraftData"},
			{"AircraftOpportunityComponent", "Core.SimpleData.EquipmentData.AircraftData.AircraftOpportunityComponent"},
			{"DiveBomberComponent", "Core.SimpleData.EquipmentData.AircraftData.DiveBomberComponent"},
			{"DiveBomberProjectile", "Core.SimpleData.EquipmentData.AircraftData.DiveBomberProjectile"},
			{"TorpedoBomberComponent", "Core.SimpleData.EquipmentData.AircraftData.TorpedoBomberComponent"},
            {"FighterComponent", "Core.SimpleData.EquipmentData.AircraftData.FighterComponent"},
            {"ScoutComponent", "Core.SimpleData.EquipmentData.AircraftData.ScoutComponent"},

            {"AircraftLaunchSystem", "Core.SimpleData.ShipData.Aircraft.AircraftLaunchSystem"},
			{"AircraftSquadron", "Core.SimpleData.ShipData.Aircraft.AircraftSquadron"},
			{"AircraftStorage", "Core.SimpleData.ShipData.Aircraft.AircraftStorage"},
			{"SquadronType", "Core.SimpleData.ShipData.Aircraft.SquadronType"},
            {"WeatherData", "Core.SimpleData.WeatherData"},
            {"SeaStateData", "Core.SimpleData.SeaStateData"},
            {"EquipImageData", "Core.SimpleData.EquipImageData"},
            {"CombatImageData", "Core.SimpleData.CombatImageData"}
        };

		private const string Recordsname = "Records";

		public static void TableFillWithData(TableRecordsClass jsonData)
		{
            if (!PathDictionary.ContainsKey(jsonData.ClassName) || !ScriptPathDictionary.ContainsKey(jsonData.ClassName))
            {
                Debug.Log(jsonData.ClassName + " not presented in dictionary");
                return;
            }
            
			var path = PathDictionary[jsonData.ClassName];
			var scriptableObject = Resources.Load(path);
			if (scriptableObject == null)
			{
				Debug.Log("EROROR " + path + " " + jsonData.ClassName);
			}

			Debug.Log(jsonData.ClassName);
			
			var tableTypeStr = string.Format(ScriptPathDictionary[jsonData.ClassName]);
			//Debug.Log(tableTypeStr);
			var tableType = Type.GetType(tableTypeStr);

			if (tableType == null)
			{
				Debug.LogError("tableTypeNotFind: " + tableTypeStr);
				return;
			}

			var field = tableType.GetFields().FirstOrDefault(x => x.Name == Recordsname);
			var objectArray = FilledData(jsonData);
			if (field == null)
			{
				Debug.LogError("NotFindField with name: " + Recordsname);
				return;
			}

			//Debug.Log(field.Name);
			field.SetValue(scriptableObject, objectArray);

#if UNITY_EDITOR
			//try to save results by SetDirty method
			UnityEditor.EditorUtility.SetDirty(scriptableObject);
#endif
		}

		private static IEnumerable<object> FilledData(TableRecordsClass jsonData)
		{
			var recordTypeStr = string.Format(ScriptPathDictionary[jsonData.ClassName] + "Record");
			//DebugLogger.Log(null, "recordTypeStr: " + recordTypeStr);
			var genericType = Type.GetType(recordTypeStr);
			if (genericType == null)
			{
				Debug.LogError("genericType: " + recordTypeStr + " is not FIND");
				return null;
			}

			var objectArray =
				(object[]) Array.CreateInstance(genericType, jsonData.ClassData.Count); // new object[jsonData.ClassData.Count];
			var j = 0;
			foreach (var record in jsonData.ClassData)
			{
				var recordFields = genericType.GetFields();
				var method =
					typeof(TablesRecordsFiller).GetMethod("GenerateInvocatedLists", BindingFlags.NonPublic | BindingFlags.Static);
				//DebugLogger.Log(null, "method: " + method);
				var generic = method.MakeGenericMethod(genericType);
				var instanceObj = generic.Invoke(null, null);
				objectArray[j] = instanceObj;

				for (var i = 0; i < recordFields.Length; i++)
				{
					if (i >= record.DataList.Count)
					{
						DebugLogger.Log(null, "Client have more field then server : " + jsonData.ClassName + "." + recordFields[i].Name,
							LogColor.Orange);
						continue;
					}

					var fieldNeedValueStr = record.DataList[i];

					FieldSetValue(recordFields[i], fieldNeedValueStr, instanceObj);
				}

				j++;
			}

			return objectArray;
		}

		private static readonly Type IntType = typeof(int);
		private static readonly Type FloatType = typeof(float);
		private static readonly Type StringType = typeof(string);
		private static readonly Type BoolType = typeof(bool);
		private static readonly Type DoubleType = typeof(double);
		private static readonly Type LongType = typeof(long);


		private static object ParseString(Type t, string fieldValue)
		{	
			if (t == LongType)
			{
				long a;
				var canParse = long.TryParse(fieldValue, out a);

				if (canParse)
				{
					return (int)a;
				}

				Debug.LogError("cantLongTypeParse: " + fieldValue);
				return null;
			}
			
			if (t == DoubleType)
			{
				double a;
				var canParse = double.TryParse(fieldValue, out a);

				if (canParse)
				{
					return a;
				}

				Debug.LogError("cantDoubleTypeParse: " + fieldValue);
				return null;
			}
			
			if (t == BoolType)
			{
                bool b;
                if (Boolean.TryParse(fieldValue, out b))
                {
                    return b;
                }

				int a;
				var canParse = int.TryParse(fieldValue, out a);

				if (canParse)
				{
					return Convert.ToBoolean(a);
				}

				Debug.LogError("cantBoolTypeParse: " + fieldValue);
				return null;
			}

			
			if (t == IntType)
			{
				int a;
				var canParse = int.TryParse(fieldValue, out a);

				if (canParse) return a;

				Debug.LogError("canIntParse: " + fieldValue);
				return -1;
			}

			if (t == FloatType)
			{
				float a;
				var canParse = float.TryParse(fieldValue, out a);

				if (canParse) return a;

				Debug.LogError("canFloatParse: " + fieldValue);
				return null;
			}
	

			if (t == StringType)
			{
				return fieldValue;
			}

			Debug.LogError("notFindThisType: " + t.Name + ";" + fieldValue +
			               "; Possible field with same index have different type on server and client");
			return null;
		}

		private static void FieldSetValue(FieldInfo curField, string fieldValue, object instance)
		{
			var fieldType = curField.FieldType;
			var valueObj = ParseString(fieldType, fieldValue);

			curField.SetValue(instance, valueObj);
		}

		// dont delete this method this method use in reflection
		// ReSharper disable once UnusedMember.Local
		private static T GenerateInvocatedLists<T>() where T : new()
		{
			return New<T>.Instance.Invoke();
		}
	}
}