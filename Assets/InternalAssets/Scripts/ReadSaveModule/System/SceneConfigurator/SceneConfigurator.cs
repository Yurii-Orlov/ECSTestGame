using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SceneConfigurator
{
	public class SceneConfigurator : ScriptableObject
	{
		public SerializableType SerializableType;
	}

	[System.Serializable]
	public enum SerializableType
	{
		None,
		Newtonsoft,
		FullSerializer,
        GameDevWareSerializer
	}

	public static class SceneConfiguratorResource
	{
		public const string Path = "SceneConfigs/SceneConfigsCore.SceneConfigurator.SceneConfigurator";

		private static SceneConfigurator _sceneConfig;

		private static void ReadSceneConfig()
		{
			_sceneConfig = Resources.Load<SceneConfigurator>(Path);
		}

		public static SceneConfigurator SceneConfig
		{
			get
			{
				if (_sceneConfig == null)
				{
					ReadSceneConfig();
				}

				return _sceneConfig;
			}
		}
	}
}
