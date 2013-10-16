using UnityEngine;
using System.Collections;


public class GoodSquadMember : SquadMember
{
	
	public GoodSquadMember(string unitClass, string name, float health = 100.0f) : base(unitClass, name, health)
	{
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		LevelInfo LevelInformations = LevelInfoObj.GetComponent<LevelInfo>();
		LevelInformations.updateNumSoldiers(+1);
	}
	
	
	public override void die()
	{
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		LevelInfo LevelInformations = LevelInfoObj.GetComponent<LevelInfo>();
		
		LevelInformations.updateNumSoldiers(-1);
		LevelInformations.updateNumSoldiersKilled(+1);
	}
}


