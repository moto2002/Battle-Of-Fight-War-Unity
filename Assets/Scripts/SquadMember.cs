using UnityEngine;
using System.Collections;

public class SquadMember
{

	public string name;
	public string unitClass;
	public float health;
	public float attackPower;

	public Texture2D SquadViewTexture;


	public SquadMember(string unitClass, string name, float health = 100.0f)
	{
		this.name = name;
		this.unitClass = unitClass;
		this.health = health;

		switch (this.unitClass) {

		case "Rifleman":
				//Debug.Log ("Class Rifleman");
				this.SquadViewTexture = Resources.Load ("Units/Rifleman") as Texture2D;
				this.attackPower = 3.0f;
				break;
			case "Slasher":
				//Debug.Log ("Class Slasher");
				this.SquadViewTexture = Resources.Load ("Units/Slasher") as Texture2D;
				this.attackPower = 1.0f;
				break;
			default:
				break;
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
