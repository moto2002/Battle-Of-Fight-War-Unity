using UnityEngine;
using System.Collections;

public class Battle : MonoBehaviour 
{
	
	//Yeah this might seem weird but bear with me
	//We have an array full of array lists
	//In practice this is an array of teams of units, where the teams can
	//shrink/grow in size as units join/leave a battle
	public ArrayList[] CombatantsByTeam;
	
	//Correspond to the relevant team in the combatants array
	public const int TEAM_GOOD_GUYS = 0;
	public const int TEAM_MONSTERS = 1;
	
	
	// Use this for initialization
	void Start () 
	{
		//Initializing all possible teams
		this.CombatantsByTeam = new ArrayList[2] {
			new ArrayList(),
			new ArrayList()
		};
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		//Animate sommat?	
	}
	
	
	//"Rounds" of combat
	public void FixedUpdate()
	{
		//Don't do this every frame in case of lagony
		
		//First go through the teams
		for (int i = TEAM_GOOD_GUYS; i < this.CombatantsByTeam.Length; i++) {
			
			int targetTeam = TEAM_MONSTERS;
			if (i == TEAM_MONSTERS) {
				targetTeam = TEAM_GOOD_GUYS;
			}
			
			//Now go through the units in the team
			foreach (GameObject UnitObj in this.CombatantsByTeam[i]) {
				
				Unit AttackingUnit = UnitObj.GetComponent<Unit>();
				//Now go through the squaddies in the unit
				//Attack a rando on the other team with each squaddie
				foreach (SquadMember Squaddie in AttackingUnit.SquadMembers) {
					
				}
			}
		}
	}
	
	
	public void addUnit(GameObject NewUnitObj)
	{
		Unit NewUnit = NewUnitObj.GetComponent<Unit>();
		//Just as a precaution check that the unit was not already added to a battle
		//Things can get repetitive since we're creating battles from the OnTriggerEnter event
		//which gets called on all units involved in the event
		if (NewUnit.getBattle() != null) {
			return;	
		}
		
		int team = TEAM_MONSTERS;
		if (NewUnitObj.tag == "GoodGuy") {
			team = TEAM_GOOD_GUYS; 	
		}
		
		this.CombatantsByTeam[team].Add(NewUnitObj);		
		
		NewUnit.createCombatEffects();
		NewUnit.setBattle(this.gameObject);
	}
	
	
}
