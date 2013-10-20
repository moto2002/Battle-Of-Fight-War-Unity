using UnityEngine;
using System.Collections;

public class Battle : MonoBehaviour 
{
	
	private int _timeOfLastRound = 0;
	
	//Yeah this might seem weird but bear with me
	//We have an array full of array lists
	//In practice this is an array of teams of units, where the teams can
	//shrink/grow in size as units join/leave a battle
	public ArrayList[] CombatantsByTeam = new ArrayList[2] {
		new ArrayList(),
		new ArrayList()
	};
	
	//Correspond to the relevant team in the combatants array
	public const int TEAM_GOOD_GUYS = 0;
	public const int TEAM_MONSTERS = 1;
	
	
	// Use this for initialization
	// Not sure if this is being called after manual Initialization
	void Start () 
	{
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
		if ((int)Time.fixedTime <= this._timeOfLastRound + 2) {
			//Debug.Log("Not attacking bc time is " + this._timeOfLastRound);
			return;
		}
		
		this._doRound();
				
		this._timeOfLastRound = (int)Time.fixedTime;
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
		
		NewUnit.setBattle(this.gameObject);
		NewUnit.createCombatEffects();
	}
	
	
	private void _doRound()
	{
		//First go through the teams
		for (int i = TEAM_GOOD_GUYS; i < this.CombatantsByTeam.Length; i++) {
			
			int targetTeam = TEAM_MONSTERS;
			if (i == TEAM_MONSTERS) {
				targetTeam = TEAM_GOOD_GUYS;
			}
			
			//Now go through the units in the team
			ArrayList TargetTeam = this.CombatantsByTeam[targetTeam];
			foreach (GameObject UnitObj in this.CombatantsByTeam[i]) {
				
				Unit AttackingUnit = UnitObj.GetComponent<Unit>();

				//Attack a rando on the other team
				int targetIndex = Random.Range(0, TargetTeam.Count - 1);
				
				//Debug.Log("ATTACKING");
				GameObject TargetUnitObj = TargetTeam[targetIndex] as GameObject;
				AttackingUnit.attack(TargetUnitObj);
				
				Unit TargetUnit = TargetUnitObj.GetComponent<Unit>();
				//Debug.Log(TargetUnit.health);
				if (TargetUnit.health <= 0.0f) {
					
					TargetTeam.RemoveAt(targetIndex);	
					if (TargetTeam.Count <= 0) { //The other team is dead oh neos!
						this._endBattle();
						return;
					}
				}
			}
		}
	}
	
	
	private void _endBattle()
	{
		Debug.Log("ENDING BATTLE");
		for (int i = TEAM_GOOD_GUYS; i < this.CombatantsByTeam.Length; i++) {
			foreach (GameObject UnitObj in this.CombatantsByTeam[i]) {
				
				Unit SurvivingUnit = UnitObj.GetComponent<Unit>();
				SurvivingUnit.setBattle(null);
			}			
		}
		
		Destroy (this);
		Destroy(this.gameObject);
	}
	
	
}
