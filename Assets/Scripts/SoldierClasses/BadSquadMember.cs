using UnityEngine;
using System.Collections;


public class BadSquadMember : SquadMember
{
	
	public BadSquadMember(string unitClass, string name, float health = 100.0f) : base(unitClass, name, health)
	{
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		LevelInfo LevelInformations = LevelInfoObj.GetComponent<LevelInfo>();
		LevelInformations.updateNumEnemies(+1);
	}
	
	
	public override void die()
	{
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		LevelInfo LevelInformations = LevelInfoObj.GetComponent<LevelInfo>();
		
		LevelInformations.updateNumEnemies(-1);
		LevelInformations.updateNumEnemiesKilled(+1);
	}
	
}


