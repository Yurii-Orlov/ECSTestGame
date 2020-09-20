using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ListItem
{
	public string task;

	public int categoryId;
	public int personId;
	public int tagId;

	public bool isComplete;
	public bool isArchived;

	public string timeAdded;
	public string timeCompleted;

	public float timeEstimate;
	public float timeSpent;

	public ListItem ()
	{
		timeAdded = DateTime.Now.ToString ();
	}

	public ListItem( int ownerId, string task )
	{
		this.categoryId = ownerId;

		this.task = task;

		timeAdded = DateTime.Now.ToString ();
	}

	public ListItem( string task, float timeEstimate, int ownerId, int personId, int categoryId )
	{
		this.categoryId = ownerId;
		this.personId = personId;
		this.tagId = categoryId;

		this.timeEstimate = timeEstimate;

		this.task = task;

		timeAdded = DateTime.Now.ToString ();
	}


}

