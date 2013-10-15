using UnityEngine;
using System.Collections;


public class Rifleman : GoodSquadMember
{
	
	public Rifleman(string name, float health = 100.0f) : base("Rifleman", name, health)
	{
		this.SquadViewTexture = Resources.Load ("Units/Rifleman") as Texture2D;
		this.attackPower = 3.0f;
	}
	
	
}


