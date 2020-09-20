using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Scriptable object window.
/// </summary>
public class ScriptableObjectClassWindow : EditorWindow
{
	private bool _scrollCountChange;
	
	private Vector2 _currentScrollPos;
	private int _scroolCount;
	private List<string> _classNames;
	
	[MenuItem("Assets/Create/ScriptableObject/Class")]
	public static void Create()
	{
		// Show the selection window.
		var window = GetWindow<ScriptableObjectClassWindow>(true, "Create a new ScriptableObjectClass", true);
		window.ShowPopup();
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
		
		_classNames = new List<string>();
		for (var i = 0; i < _scroolCount; i++)
		{
			_classNames.Add(string.Empty);
		}
	}
	
	private void DrawScroll()
	{
		GUILayout.Label("Begin Scroll");
		_currentScrollPos = EditorGUILayout.BeginScrollView(_currentScrollPos);
		for (var i = 0; i < _scroolCount; i++)
		{
			_classNames[i] = EditorGUILayout.TextField(_classNames[i]);
		}
		EditorGUILayout.EndScrollView();
	}

	private void DrawButton()
	{
		if (!GUILayout.Button("Create")) return;
		
		ScriptableObjectClassFactory.CreateClass(_classNames);
		Close();
	}
}