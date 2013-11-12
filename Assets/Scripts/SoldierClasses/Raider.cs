using UnityEngine;
using System.Collections;


public class Raider : BadSquadMember
{
	
	public Raider(string name, float health = 100.0f) : base("Raider", name, health)
	{
		this.SquadViewTexture = Resources.Load ("Units/Raider") as Texture2D;
		this.attackPower = 2.0f;
	}
	
	
}


