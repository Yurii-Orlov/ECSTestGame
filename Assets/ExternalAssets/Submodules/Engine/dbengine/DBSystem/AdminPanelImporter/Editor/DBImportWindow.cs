using System;
using System.Linq;
using CustomLogger;
using UnityEditor;
using UnityEngine;

namespace Core.Database
{
	public class DbImportWindow : EditorWindow
	{
		private const string SpaceSpecial = "%20";
		
		private const string SchemeStatic = "\"json_scheme\":{";
		private const string DataStatic = "\"json_data\":{";
		private TextAsset _sourceTxt;
		
		[MenuItem("Database/Import")]
		public static void Create()
		{
			// Show the selection window.
			var window = GetWindow<DbImportWindow>(true, "Import ScriptableObjects", true);
			window.ShowPopup();
		}
		
		public void OnGUI()
		{
			DrawInputField();
			FilledData();
		}

		private void DrawInputField()
		{
			_sourceTxt = (TextAsset)EditorGUILayout.ObjectField(_sourceTxt, typeof(TextAsset), false);
		}

		private string GetJsonScheme()
		{
			var srcTxt = _sourceTxt.text;
			var clearedText = srcTxt.Replace("\n", "");
			clearedText = clearedText.Replace("\t", "");
			clearedText = clearedText.Replace(" ", "");
			
			var removedBrackets = clearedText.Remove(0, 1);
			removedBrackets = removedBrackets.Remove(removedBrackets.Length - 1);
			
			var stringsSpliited = ParseDataScheme.ParseScheme(removedBrackets);

			var jsonScheme = stringsSpliited[0];
			jsonScheme = jsonScheme.Remove(jsonScheme.Length - 1);
			var titleIndex = jsonScheme.IndexOf(SchemeStatic, StringComparison.Ordinal);
			jsonScheme = jsonScheme.Remove(titleIndex, SchemeStatic.Length);

			return jsonScheme;
		}
		
		private string GetJsonData()
		{
			var srcTxt = _sourceTxt.text;
			var clearedText = srcTxt.Replace("\n", "");
			clearedText = clearedText.Replace("\t", "");
			clearedText = clearedText.Replace(" ", "");
			clearedText = clearedText.Replace(SpaceSpecial, " ");
			
			var removedBrackets = clearedText.Remove(0, 1);
			removedBrackets = removedBrackets.Remove(removedBrackets.Length - 1);
			
			var stringsSpliited = ParseDataScheme.ParseScheme(removedBrackets);
			
			var jsonData = stringsSpliited[1];
			jsonData = jsonData.Remove(jsonData.Length - 1);
			//Debug.Log(jsonData);
			var titleIndex = 0;
			jsonData = jsonData.Remove(titleIndex, DataStatic.Length);
			//Debug.Log(jsonData);
			//Debug.Log(titleIndex);
			return jsonData;
		}

		private void FilledData()
		{
			if (!GUILayout.Button("FilledData")) return;

			//var jsonScheme = GetJsonScheme();
			var jsonData = GetJsonData();
			//var schemeRecords = ParseDataScheme.ParseSchemeTest(jsonScheme).ToList();
			var dataRecords = ParseDataData.ParseData(jsonData);
			dataRecords.ToList().ForEach(TablesRecordsFiller.TableFillWithData);
		}
	}
}