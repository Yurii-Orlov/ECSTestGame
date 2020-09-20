using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

internal class EndNameEdit : EndNameEditAction
{
	#region implemented abstract members of EndNameEditAction
	public override void Action (int instanceId, string pathName, string resourceFile)
	{
		AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
	}

	#endregion
}

/// <summary>
/// Scriptable object window.
/// </summary>
public class ScriptableObjectWindow : EditorWindow
{
	//private int selectedIndex;

	private bool _scrollCountChange;
	private Vector2 _currentScrollPos;
	private int _scroolCount;
	private int[] _selectedIndexes;
	
	private string[] _names;
	
	private Type[] _types;
	
	public Type[] Types
	{ 
		get { return _types; }
		set
		{
			_types = value;
			_names = _types.Select(t => t.FullName).ToArray();
		}
	}

	public static void CreateScriptableObjectAsset(Type instanceType, string assetname)
	{
		//DebugLogger.Log(null, "instanceType: " + instanceType.FullName);
		var asset = CreateInstance(instanceType);
		//DebugLogger.Log(null, "asset: " + asset);
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
			asset.GetInstanceID(),
			CreateInstance<EndNameEdit>(),
			string.Format("{0}.asset", assetname),
			AssetPreview.GetMiniThumbnail(asset), 
			null);
	}
	
	public static void CreateScriptableObjectAsset(List<ScriptableCreateData> scriptableDataList)
	{
		foreach (var createData in scriptableDataList)
		{
			CreateScriptableObjectAsset(createData.InstanceType, createData.Assetname);
		}
	}

	public class ScriptableCreateData
	{
		public Type InstanceType;
		public string Assetname;
	}
	
	public void OnGUI()
	{
		CheckScrollElementsCount();
		InitNewListValues();
		
		DrawScroll();
		DrawButton();
	}
	
	private void CheckScrollElementsCount()
	{
		_scrollCountChange = false;
		var prevScrollCount = _scroolCount;
		
		GUILayout.Label("EnterScrollNumber");
		_scroolCount =  EditorGUILayout.IntField(_scroolCount);

		if (prevScrollCount != _scroolCount)
		{
			_scrollCountChange = true;
		}
	}
	
	private void InitNewListValues()
	{
		if (!_scrollCountChange) return;

		_selectedIndexes = new int[_scroolCount];
	}

	private void DrawScroll()
	{
		GUILayout.Label("Begin Scroll");
		_currentScrollPos = EditorGUILayout.BeginScrollView(_currentScrollPos);
		for (var i = 0; i < _scroolCount; i++)
		{
			_selectedIndexes[i] = EditorGUILayout.Popup(_selectedIndexes[i], _names);
		}
		EditorGUILayout.EndScrollView();
	}

	private List<ScriptableCreateData> CreateScriptableCreateList()
	{
		var list = new List<ScriptableCreateData>();
		for (var i = 0; i < _scroolCount; i++)
		{
			var selectedIndex = _selectedIndexes[i];
			var createData = new ScriptableCreateData
			{
				Assetname = _names[selectedIndex],
				InstanceType = _types[selectedIndex]
			};
			list.Add(createData);
		}
		return list;
	}
	
	private void DrawButton()
	{
		if (!GUILayout.Button("Create")) return;

		var createList = CreateScriptableCreateList();
		CreateScriptableObjectAsset(createList);
		Close();
	}
}