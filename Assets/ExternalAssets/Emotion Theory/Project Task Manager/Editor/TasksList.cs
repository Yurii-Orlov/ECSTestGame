using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TasksList : EditorWindow
{
	private static TasksList _window;
	private ListData _listData;

	// STATE
	private int _currentCategoryIndex = 0;
	private int _currentStatusIndex = 0;
	private int _currentPersonIndex = 0;
	private int _currentTagIndex = 0;
//	private string _currentSearchFilter = ""; // TODO: Planned future feature.

	// NEW TASK
	private string _newTask;
	private int _newCategoryIndex = 0;
	private int _newPersonIndex = 0;
	private int _newTagIndex = 0;
	private float _newTimeEstimate = 2f;

	private Vector2 _scrollPosition = Vector2.zero;

	public bool saveable;

	private bool showCompletedTasks 
	{ get { return _currentStatusIndex == 0 || _currentStatusIndex == 2; } }

	private bool showAll
	{ get { return _currentCategoryIndex == 0; } }

	private bool showActiveTasks
	{ get { return _currentStatusIndex == 0 || _currentStatusIndex == 1; } }

	private string scriptPath
	{ get { return AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)); } }

	public string assetPath
	{
		get
		{
			return System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(scriptPath));
		}
	}

	private string dataPath
	{ get { return assetPath + "/Tasks.asset"; } }
	
	[MenuItem ("Window/Tasks %l")]
    public static void Init ()
    {
        // Get existing open window or if none, make a new one:
        _window = ( TasksList )EditorWindow.GetWindow (typeof ( TasksList ));
        // This is silly since OnEnable will do this but avoid the warning about unused _window variable.
        _window.titleContent.text = "Tasks";
    }

	private void OnEnable ()
	{

		titleContent.text = "Tasks";
		autoRepaintOnSceneChange = false;
		LoadDataAsset ();
	}

	public void LoadDataAsset ()
	{
		// Create our data if we have none.
		if(_listData == null)
		{
			//Debug.Log("no asset file found, need to reload");
			_listData = AssetDatabase.LoadAssetAtPath( dataPath, typeof(ListData)) as ListData;
			if(_listData == null)
			{
				//Debug.Log("no asset file found, could not reload");	
				_listData = ScriptableObject.CreateInstance(typeof(ListData)) as ListData;
				AssetDatabase.CreateAsset(_listData, dataPath );
				GUI.changed = true;				
			}						
		}
	}

	public static T[] InsertIntoArray <T>(T[] array, int index, T item)
	{
		var list = array.ToList ();
		list.Insert (index, item);

		return list.ToArray();
	}

	public static T[] AddToArray <T>(T[] array, T item)
	{
		var list = array.ToList ();
		list.Add (item);

		return list.ToArray();
	}

	public static Color ColorWithAlpha (Color color, float alpha)
	{
		return new Color (color.r, color.g, color.b, alpha);
	}

	protected virtual void DrawItemRow (int i, ListItem item, GUIStyle itemStyle)
	{
		var cat = _listData.categories [item.categoryId];

		Texture2D tex = new Texture2D (1, 1, TextureFormat.RGBA32, false);
		tex.SetPixel (0, 0, ColorWithAlpha(cat.color, 0.5f));
		tex.Apply ();

		GUIStyle s = new GUIStyle(); 
		if (cat.trackTime && !item.isComplete)
			s.normal.background = tex;

		EditorGUILayout.BeginVertical ( s );
		{
			EditorGUILayout.BeginHorizontal ();
			{
				// CHECKMARK
				if (EditorGUILayout.Toggle (item.isComplete, GUILayout.Width (20)))
				{
					if (!item.isComplete)
						_listData.CompleteTask (item);
				}
				else
				{
					if (item.isComplete)
						_listData.ReactivateTask (item);
				}

				// TEXT
				if (!cat.trackTime)
					itemStyle.normal.textColor = cat.color;
				item.task = EditorGUILayout.TextArea (item.task, itemStyle);


			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.Space ();

				// CATEGORY
				item.categoryId = EditorGUILayout.Popup (item.categoryId, _listData.categoryStrings, GUILayout.Width (60));

				// TYPE
				item.tagId = EditorGUILayout.Popup (item.tagId, _listData.tagStrings, GUILayout.Width (60));

				// PERSON
				item.personId = EditorGUILayout.Popup (item.personId, _listData.peopleStrings, GUILayout.Width (60));

				// TIME ESTIMATE
				item.timeEstimate = EditorGUILayout.FloatField (item.timeEstimate, GUILayout.Width (32));
				item.timeEstimate = Mathf.Clamp (item.timeEstimate, 0f, 8f);

				// ARCHIVE
				if (item.isComplete)// || true) // LOL remove the "true" if you only want archive options when complete. 
				{
					if (!item.isArchived && GUILayout.Button ("archive", GUILayout.Width (68)))
						_listData.ArchiveTask (item);
					else if (item.isArchived && GUILayout.Button ("unarchive", GUILayout.Width (68)))
						_listData.tasks [i].isArchived = false;
				}

				// DELETE
				if (GUILayout.Button ("x", GUILayout.Width (23)))
				{
					_listData.tasks.RemoveAt (i);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndVertical ();
	}

	public void OnGUI ()
	{
//		EditorGUILayout.BeginVertical ();

		GUIStyle titleGUIStyle = new GUIStyle ();
		titleGUIStyle.fontSize = 128;
		titleGUIStyle.fontStyle = FontStyle.BoldAndItalic;
		titleGUIStyle.normal.textColor = ColorWithAlpha (Color.black, 0.05f);
		EditorGUILayout.LabelField("TASKS", titleGUIStyle);

		// owners
		string[] categories = InsertIntoArray<string> (_listData.categoryStrings, 0, "All");
		categories = AddToArray<string> (categories, "Archived");

		// people
		string[] people = InsertIntoArray<string> (_listData.peopleStrings, 0, "All");

		// categories
		string[] tags = InsertIntoArray<string> (_listData.tagStrings, 0, "All");

		// status
		string[] statuses = new string[] { "All", "Active only", "Completed only" };

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("CATEGORY", EditorStyles.boldLabel, GUILayout.Width (88));
			_currentCategoryIndex = EditorGUILayout.Popup (_currentCategoryIndex, categories);
			EditorGUILayout.EndVertical ();

			EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("STATUS", EditorStyles.boldLabel, GUILayout.Width (64));
			_currentStatusIndex = EditorGUILayout.Popup (_currentStatusIndex, statuses);
			EditorGUILayout.EndVertical ();

			EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("PERSON", EditorStyles.boldLabel, GUILayout.Width (64));
			_currentPersonIndex = EditorGUILayout.Popup (_currentPersonIndex, people);
			EditorGUILayout.EndVertical ();

			EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("TYPE", EditorStyles.boldLabel, GUILayout.Width (36));
			_currentTagIndex = EditorGUILayout.Popup (_currentTagIndex, tags);
			EditorGUILayout.EndVertical ();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		// LIST OF ITEMS
		GUIStyle itemStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
		itemStyle.alignment = TextAnchor.UpperLeft;
		itemStyle.fontStyle = FontStyle.Bold;
		itemStyle.fontSize = 12;

		_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
		{
			int displayCount = 0;

			bool showArchives = _currentCategoryIndex == categories.Length - 1;

			// First, draw all the active quests.
			if (showActiveTasks)
			{
				for (int i = 0; i < _listData.tasks.Count; i++)
				{
					ListItem item = _listData.tasks [i];
					ListItemOwner cat = _listData.categories [item.categoryId];
					var catIndex = _currentCategoryIndex - 1;

					// Show active only.
					if (item.isComplete == true)
						continue;
					
					// Only show the item if we're in the correct category, showing ALL, or showing ARCHIVES
					if (!showAll && catIndex != item.categoryId && !showArchives)
						continue;

					// Only show archived items if we're archived.
					if (item.isArchived != showArchives)
						continue;

					// Only show items if we have the correct person.
					if (_currentPersonIndex != 0 && _currentPersonIndex - 1 != item.personId)
						continue;

					// Only show items if we have the correct category.
					if (_currentTagIndex != 0 && _currentTagIndex - 1 != item.tagId)
						continue;
					
//					int adjustedIndex = _currentOwnerIndex - 1;
//					owner = _listData.owners [adjustedIndex];

//					itemStyle.normal.textColor = owner.color;

					DrawItemRow (i, item, itemStyle);

					displayCount++;

					EditorGUILayout.Space ();
					EditorGUILayout.Space ();
				}
			} 

			if (showCompletedTasks)
			{
				for( int i = 0; i < _listData.tasks.Count; i++)
				{
					ListItem item = _listData.tasks[i];
					ListItemOwner cat = _listData.categories [item.categoryId];
					var catIndex = _currentCategoryIndex - 1;

					// Show completed only.
					if (item.isComplete == false)
						continue;

					// Only show the item if we're in the correct category.
					if (!showAll && catIndex != item.categoryId && !showArchives)
						continue;

					// Only show archived items if we're archived.
					if (item.isArchived != showArchives)
						continue;

					// Only show items if we have the correct person.
					if (_currentPersonIndex != 0 && _currentPersonIndex - 1 != item.personId)
						continue;

					// Only show items if we have the correct category.
					if (_currentTagIndex != 0 && _currentTagIndex - 1 != item.tagId)
						continue;

					itemStyle.normal.textColor = Color.gray;
					itemStyle.fontStyle = FontStyle.Italic;

					DrawItemRow (i, item, itemStyle);

					displayCount++;

					EditorGUILayout.Space();
					EditorGUILayout.Space ();
				}
			}	 

			// Then, draw all the completed quests.
			if (displayCount <= 0)
			{
				EditorGUILayout.LabelField ("No tasks currently", itemStyle);
			} 
		}
		EditorGUILayout.EndScrollView();

		// CREATE NEW TASK
		var nts = EditorStyles.boldLabel; 
		GUILayout.Label ("NEW TASK", nts);
//		EditorGUILayout.LabelField ("NEW TASK", titleGUIStyle);
		EditorGUILayout.BeginHorizontal();
		{
//			EditorGUILayout.LabelField("Create Task:", EditorStyles.boldLabel);
			_newCategoryIndex  = EditorGUILayout.Popup(_newCategoryIndex, _listData.categoryStrings);//, GUILayout.Width(90));
			_newPersonIndex  = EditorGUILayout.Popup(_newPersonIndex, _listData.peopleStrings);//, GUILayout.Width(90));
			_newTagIndex = EditorGUILayout.Popup(_newTagIndex, _listData.tagStrings);//, GUILayout.Width(90));

			_newTimeEstimate = EditorGUILayout.Slider (_newTimeEstimate, 0f, 8f);
		}
		EditorGUILayout.EndHorizontal();
		_newTask = EditorGUILayout.TextArea(_newTask, GUILayout.Height(40));

		// BOTTOM BUTTONS
		GUILayout.BeginHorizontal ();
		{
			if ((GUILayout.Button ("Create Task") && _newTask != "")) 
			{
				// create new task
//				_listData.AddTask (_newTaskOwnerIndex, _newTask);
				_listData.AddTask (_newTask, _newTimeEstimate, _newCategoryIndex, _newPersonIndex, _newTagIndex );
				//EditorUtility.DisplayDialog("Task created for " + newOwner.name, _newTask, "Sweet");
				_newTask = "";
				GUI.FocusControl (null);				
			}

			if ((GUILayout.Button ("Create Multiple Tasks") && _newTask != "")) {
				var ss = _newTask.Split ('\n');

				foreach (var s in ss) {
					if (string.IsNullOrEmpty (s))
						continue;

					string sss = s;

					if (sss.StartsWith ("- "))
						sss = sss.Replace ("- ", "\t");
					
					// create new task
					_listData.AddTask (sss, _newTimeEstimate, _newCategoryIndex, _newPersonIndex, _newTagIndex );			
				}
				_newTask = "";
				GUI.FocusControl (null);	
								
			}
		}
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		{
			if (GUILayout.Button ("Copy Current")) {
				string s = "";

				foreach (var l in _listData.tasks) 
				{
					if (!l.isComplete && (_currentCategoryIndex == 0 || l.categoryId == _currentCategoryIndex))
						s += l.task.Replace ("\t", "- ") + "\n";
				}

				EditorGUIUtility.systemCopyBuffer = s;
			}
//			if (GUILayout.Button ("Copy Completed")) {
//				string s = "";
//
//				foreach (var l in _listData.items) {
//					if (l.isComplete)
//						s += l.task + "\n";
//				}
//
//				EditorGUIUtility.systemCopyBuffer = s;
//			}
			if (GUILayout.Button ("Archive Completed")) 
			{
				for (int i = 0; i < _listData.tasks.Count; i++) 
				{
					var l = _listData.tasks [i];
					if (l.isComplete && !l.isArchived) {
						_listData.ArchiveTask (i);
					}
				}
			}
			if (GUILayout.Button ("Remove Archived")) 
			{
				if (EditorUtility.DisplayDialog (
					    "Careful", 
					    "You are about to remove all archived items. This cannot be undone.",
					    "Remove all archived items!",
					    "Cancel"))
				{
					RemoveAllArchivedItems ();
				}
			}

		}
		GUILayout.EndHorizontal ();


//		EditorGUILayout.EndVertical ();


		
//		if(GUI.changed)
//		{
//			//Debug.Log("Save Data: " + _listData.items.Count);
//			EditorUtility.SetDirty(_listData);
//			EditorApplication.SaveAssets();
//			AssetDatabase.SaveAssets();		
//		}	
		if (GUIUtility.hotControl != 0)
			saveable = true;
		if (GUIUtility.hotControl == 0 && saveable)
		{
			saveable = false;
			// Debug.Log("Save Data: " + _listData.items.Count);
			EditorUtility.SetDirty(_listData);
			AssetDatabase.SaveAssets();
		}


	}

	void RemoveAllArchivedItems ()
	{
		for (int i = 0; i < _listData.tasks.Count; i++)
		{
			var l = _listData.tasks [i];
			if (l.isArchived)
			{
				_listData.tasks.RemoveAt (i);
				i--;
			}
		}
	}


	void OnDestroy()
	{
		EditorUtility.SetDirty(_listData);
		AssetDatabase.SaveAssets();
		AssetDatabase.SaveAssets();
	}	
}
