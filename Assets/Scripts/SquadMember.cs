using UnityEngine;
using System.Collections;

public class SquadMember
{

	public string name;
	public string unitClass;
	public float health;


	public SquadMember(string name, string unitClass, float health = 100.0f)
	{
		this.name = name;
		this.unitClass = unitClass;
		this.health = health;
	}

	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
