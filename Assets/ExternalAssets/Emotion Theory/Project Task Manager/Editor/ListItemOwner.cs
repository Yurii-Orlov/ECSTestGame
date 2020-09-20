using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ListItemOwner
{
	public string name;
	public Color color;
//	public int index;
	public Texture2D icon;
	public bool trackTime;

	public ListItemOwner ()
	{

	}

	public ListItemOwner( string name, Color color, int index = 0)
	{
		this.name = name;
		this.color = color;
//		this.index = index;
	}

	public ListItemOwner( string name, Color color , int index, Texture2D icon, bool trackTime)
	{
		this.name = name;
		this.color = color;
//		this.index = index;
		this.icon = icon;
		this.trackTime = trackTime;
	}
}
