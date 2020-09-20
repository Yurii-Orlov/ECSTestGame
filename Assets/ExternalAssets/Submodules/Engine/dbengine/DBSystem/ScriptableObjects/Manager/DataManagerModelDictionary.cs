using System;
using System.Collections.Generic;
using CustomLogger;
using UnityEngine;

namespace Core.DataManagement
{
	public class DataManagerModelDictionary
	{
		private const string ScriptableObjectsPaths = "ScriptableObjects/";
		private Dictionary<string, object> _objectDic;

		private void LazyCached()
		{
			if (_objectDic != null) return;

			_objectDic = new Dictionary<string, object>();
			var results = Resources.LoadAll(ScriptableObjectsPaths);
			foreach (var res in results)
			{
				var resType = res.GetType().FullName;
				if (resType != null) _objectDic[resType] = res;
			}
		}

		public T GetScriptableObject<T>() where T : ScriptableObject
		{
			LazyCached();

			var typeName = typeof(T).FullName;
			var contains = _objectDic.ContainsKey(typeName);
			if (contains)
			{
				return (T) _objectDic[typeName];
			}

			DebugLogger.Error(this, "not find " + typeName + " in Resources/" + ScriptableObjectsPaths);
			return null;
		}
	}
}