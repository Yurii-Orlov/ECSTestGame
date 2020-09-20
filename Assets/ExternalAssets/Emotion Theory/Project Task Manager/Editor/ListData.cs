using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ListData : ScriptableObject
{
	public List<ListItemOwner> categories = new List<ListItemOwner>();
	public List<ListItemOwner> people = new List<ListItemOwner>();
	public List<ListItemOwner> tags = new List<ListItemOwner>();

	public List<ListItem> tasks = new List<ListItem>();

	public string[] categoryStrings
	{ get { return categories.Select ( x => x.name ).ToArray (); } }

	public string[] peopleStrings
	{ get { return people.Select ( x => x.name ).ToArray (); } }

	public string[] tagStrings
	{ get { return tags.Select ( x => x.name ).ToArray (); } }

	
	public ListData ()
	{
		// create over list owners, can be an editor window later		
		categories.Add( new ListItemOwner("Normal", Color.black, 0) );		
		categories.Add( new ListItemOwner("Urgent", Color.red, 1) );
		categories.Add( new ListItemOwner("In Progress", Color.cyan, 2, null, true) );
		categories.Add( new ListItemOwner("Note", Color.green, 3) );	

		people.Add( new ListItemOwner("Default", Color.black, 0) );	

		tags.Add( new ListItemOwner("Programming", Color.black, 0) );		
		tags.Add( new ListItemOwner("Art", Color.red, 1) );
		tags.Add( new ListItemOwner("Level Design", Color.cyan, 2) );
		tags.Add( new ListItemOwner("Other", Color.green, 3) );	
	}

	public void AddTask ( string task, float timeEstimate, int ownerId, int personId, int categoryId)
	{
		ListItem item = new ListItem( task, timeEstimate, ownerId, personId, categoryId );
		tasks.Add(item);
	}

	public void AddTask( int ownerId, string task)
	{
		ListItem item = new ListItem( ownerId, task );
		tasks.Add(item);
	}

	public void CompleteTask (int index)
	{
		if (index < 0 || index >= tasks.Count)
			return;
		
		var item = tasks [index];
		CompleteTask (item);
	}

	public void ReactivateTask (ListItem item)
	{
		item.isComplete = false;
		item.timeCompleted = "";
		item.isArchived = false;
	}
		
	public void CompleteTask (ListItem item)
	{
		item.isComplete = true;
		item.timeCompleted = DateTime.Now.ToString ();
	}

	public void ArchiveTask (int index)
	{
		if (index < 0 || index >= tasks.Count)
			return;

		var item = tasks [index];
		ArchiveTask (item);
	}

	public void ArchiveTask (ListItem item)
	{
		item.isArchived = true;
	}

}
	