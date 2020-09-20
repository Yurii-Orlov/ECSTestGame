﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using CsvParser;
using CustomLogger;
using UnityEngine;

namespace Core.SimpleData
{
	
	#region SimpleStructure
	[Serializable]
	public abstract class SimpleDataRecord
	{
		public const int IdError = -1;
		public static int MaxId;

		protected SimpleDataRecord()
		{
			
		}

		protected SimpleDataRecord(bool parsed)
		{
			if (parsed)
			{
				//DebugLogger.Log(this, "MaxId: " + MaxId);
				MaxId++;
			}
		}
	}
	
	public abstract class SimpleTableScriptableObject<T> : ScriptableObject where T: SimpleDataRecord
	{
		public T[] Records;

		protected abstract string GetRecordSource();
		protected abstract T ParseOneRecord(IEnumerable<string> record);

		protected virtual List<T> ParseOneStrokeWithMultipleRecords(IEnumerable<string> record)
		{
			return null;
		}

		protected virtual void ParseWithAppend()
		{
			var records = GetRecords();
			
			var recordList = records.ToList();
			var currentList = Records.ToList();
			currentList.AddRange(recordList);

			Records = currentList.ToArray();

			SetScriptableDirty();
		}

		protected virtual void Parse()
		{
			Records = GetRecords();

			SetScriptableDirty();
		}

		private T[] GetRecords()
		{
			var sourceData = GetRecordSource();
			if (sourceData == null)
			{
				Debug.LogError("SourceData is null");
				return null;
			}

			SimpleDataRecord.MaxId = 0;

			var allStringStrokes = CsvParserStatic.GetAllRecordsSkipHeaders(sourceData);
			
			var recordResultList = new List<T>();
			foreach (var stroke in allStringStrokes)
			{
				var enumerable = stroke as IList<string> ?? stroke.ToList();
				
				var recordData = ParseOneRecord(enumerable);
				var recordDataList = ParseOneStrokeWithMultipleRecords(enumerable);
				recordResultList.Add(recordData);
				
				if(recordDataList == null) continue;
				
				recordResultList.AddRange(recordDataList);
			}
			
			var records = recordResultList.Where(x => x != null).ToArray();

			return records;
		}

		public void SetScriptableDirty()
		{
#if UNITY_EDITOR
			//try to save results by SetDirty method
			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}
	}
	#endregion
}