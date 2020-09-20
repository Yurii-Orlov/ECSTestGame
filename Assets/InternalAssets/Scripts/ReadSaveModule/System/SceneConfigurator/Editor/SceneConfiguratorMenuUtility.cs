using Core.Utilities;
using UnityEngine;
using UnityEditor;

namespace Core.SceneConfigurator
{
	public class SceneConfiguratorUtility
	{
		[MenuItem("Assets/Create/SceneConfigurator")]
		public static void CreateAsset()
		{
			ScriptableObjectUtility.CreateAsset<SceneConfigurator>();
			PlayerPrefs.Save();
		}
	}
}