using System;
using System.Collections.Generic;
using System.Linq;
using CustomLogger;
using InternalAssets.Scripts.Engine.Utilities;
using UnityEngine;

namespace Core.ModelManagement
{
	public class ModelController
	{
		public const string DataPath = "Data";
		public static string PersistanceDataPath;

		public static string GetPathForData()
		{
			var persistantDataPath = PersistanceDataPath;
			if (persistantDataPath == null)
			{
				Debug.LogError("Add StartSceneMonoFile to scene and call this function after Awake(start or else)");
				DebugLogger.Error(null, "Add StartSceneMonoFile to scene and call this function after Awake(start or else)", LogColor.Red);
				return "";
			}
			return PathUtils.Combine(Application.persistentDataPath, DataPath);
		}

		private readonly List<WeakReference> _allData;

		public ModelController()
		{
			_allData = new List<WeakReference>();
			PersistanceDataPath = Application.persistentDataPath;
		}

		public void Register(BaseModelObject model)
		{
			lock (_allData)
			{
				_allData.Add(new WeakReference(model));
			}
		}

		public void UnRegister(BaseModelObject model)
		{
			lock (_allData)
			{
				_allData.RemoveAll(wr => ReferenceEquals(wr.Target, model));
			}
		}

		public void UnRegisterAll()
		{
			lock (_allData)
			{
				_allData.Clear();
			}
		}

		public void WriteAll()
		{

            lock (_allData)
			{
				var dataList = _allData.ToList();
				foreach (var t in dataList)
				{
					var wr = t;
					if (wr.IsAlive)
					{
						var handler = t.Target as IData;
						if (handler != null)
						{
							handler.Write();
						}
						else
						{
							Debug.Log("handler is null");
							_allData.Remove(wr);
						}
					}
					else
					{
						_allData.Remove(wr);
					}
				}
			}
		}
	}
}