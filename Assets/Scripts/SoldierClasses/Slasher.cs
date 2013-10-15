using UnityEngine;
using System.Collections;


public class Slasher : BadSquadMember
{
	
	public Slasher(string name, float health = 100.0f) : base("Slasher", name, health)
	{
		this.SquadViewTexture = Resources.Load ("Units/Slasher") as Texture2D;
		this.attackPower = 1.0f;
	}
	
	
}


