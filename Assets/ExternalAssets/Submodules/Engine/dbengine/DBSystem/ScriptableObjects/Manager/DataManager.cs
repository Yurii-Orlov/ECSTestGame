using Core.Management;
using UnityEngine;

namespace Core.DataManagement
{
	public class DataManager : IManager
	{
		private DataManagerModel _dataManagerModel;
		private DataManagerModelDictionary _dataManagerModelDictionary;

		public void Init()
		{
			_dataManagerModelDictionary = new DataManagerModelDictionary();
		}

		public T GetScriptableObjectDictionary<T>() where T : ScriptableObject
		{
			return _dataManagerModelDictionary.GetScriptableObject<T>();
		}
	}
}