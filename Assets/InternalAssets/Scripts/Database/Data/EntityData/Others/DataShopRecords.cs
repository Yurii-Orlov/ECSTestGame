using OrCor_GameName;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SimpleData
{
	public class DataShopRecords : SimpleTableScriptableObject<DataShopRecord>
	{
		public TextAsset SourceData;

		[ContextMenu("Parse")]
		protected override void Parse()
		{
			base.Parse();
		}

		protected override string GetRecordSource()
		{
			return SourceData.text;
		}

		protected override DataShopRecord ParseOneRecord(IEnumerable<string> record)
		{
			var enumerator = record.GetEnumerator();
			enumerator.MoveNext();

			//get first value from IEenumerable
			var firstEnumerableValue = enumerator.Current;
			var recordName = firstEnumerableValue;
			//DebugLogger.Log(null, "tableValue: " + tableValue, LogColor.Chartreuse);
			enumerator.Dispose();

			var dataShopRecordsRecord = new DataShopRecord();
			return dataShopRecordsRecord;
		}
	}

	[Serializable]
	public class DataShopRecord : SimpleDataRecord
	{
        public int Id;

        public int Price;

        public Enumerators.ShopReapeatType RepeatType;

        public string Title;

        public Sprite ShopSprite;
    }
}
