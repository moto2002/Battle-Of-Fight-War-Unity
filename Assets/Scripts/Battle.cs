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
	
	}
	
	
	//"Rounds" of combat
	public void FixedUpdate()
	{
		foreach (ArrayList Team in this.CombatantsByTeam) {
			
			foreach (GameObject Unit in Team) {
				
			}
		}
	}
	
	
	public void addUnit(GameObject NewUnit, int team)
	{
		this.CombatantsByTeam[team].Add(NewUnit);		
		
		Unit UnitGuy = NewUnit.GetComponent<Unit>();
		UnitGuy.createCombatEffects();
	}
	
	
}
