using UnityEngine;


public class SquadMemberCreator
{

	public SquadMemberCreator()
	{
	}
	
	
	public SquadMember createSquadMember(string unitClass, string name)
	{
		switch (unitClass) {
			
			case "Slasher":
				return new Slasher(name);
			case "Raider":
				return new Raider(name);
			default: //Rifleman	
				return new Rifleman(name);
		
		} //end switch
	}
	
	
}


