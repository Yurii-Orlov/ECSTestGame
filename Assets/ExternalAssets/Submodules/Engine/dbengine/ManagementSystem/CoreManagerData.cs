using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Core.Management
{
	public class CoreManagerData : ICoreManagerData
	{
		private List<BasicManagerData> _managerList;

		private void CheckForNullManagerList()
		{
			if (_managerList == null)
			{
				_managerList = new List<BasicManagerData>();
			}
		}

		public T CreateInstanceT<T>() where T : IManager, new()
		{
			return new T();
		}

		public void Init<T>() where T : IManager, new()
		{
			CheckForNullManagerList();

			var searchType = typeof(T);

			if (_managerList.Select(managerData => managerData.Manager)
				.Select(manager => manager.GetType())
				.Any(managerType => searchType == managerType))
			{
				return;
			}

			CreateBasicData<T>();
		}

		public void ClearAll()
		{
			_managerList = null;
		}

		private T CreateBasicData<T>() where T : IManager, new()
		{
			CheckForNullManagerList();

			var foundManager = CreateInstanceT<T>();
			foundManager.Init();

			var basicManagerData = new BasicManagerData { Manager = foundManager };
			_managerList.Add(basicManagerData);

			return foundManager;
		}

		public T GetManager<T>() where T : IManager, new()
		{
			CheckForNullManagerList();

			var searchType = typeof(T);
			
			foreach (var managerData in _managerList)
			{
				var manager = managerData.Manager;
				var managerType = manager.GetType();

				if (managerType == searchType)
				{
					return (T)manager;
				}
			}

			var foundManager = CreateBasicData<T>();

			return foundManager;
		}

		public void RegisterManager<T>(T customManager) where T: IManager
		{
			var basicManagerData = new BasicManagerData { Manager = customManager };
			_managerList.Add(basicManagerData);
		}

		public void UnRegisterManager<T>(T customManager) where T : IManager
		{
			var searchType = typeof(T);
			BasicManagerData foundManager = null;
			foreach (var managerData in _managerList)
			{
				var manager = managerData.Manager;
				var managerType = manager.GetType();

				if (managerType == searchType)
				{
					foundManager = managerData;
					break;
				}
			}

			if(foundManager != null)
				_managerList.Remove(foundManager);
		}
	}

	public class BasicManagerData
	{
		public IManager Manager;
	}
}
