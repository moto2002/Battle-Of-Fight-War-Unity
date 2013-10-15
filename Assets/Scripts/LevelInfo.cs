using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class LevelInfo : MonoBehaviour 
{

	public int minSpawnsFromEnemySpawner;
	public int maxSpawnsFromEnemySpawner;
	public int timeBetweenSpawns;

	public int numObjectivesCaptured;
	public int totalNumObjectives;
	public int gameEvent;
	
	public int numSoldiers;
	public int numSoldiersKilled;
	
	public int numEnemiesRemaining;
	public int numEnemiesKilled;
	

	public Vector3 StatusUpdateLocation;

	public TextAsset Names;
	private string[] _possibleNames;

	public const int GAME_EVENT_NONE = 0;
	public const int GAME_EVENT_PAUSED = 1;
	public const int GAME_EVENT_PLAYER_WON = 2;
	public const int GAME_EVENT_PLAYER_LOST = 3;

	public const int GAME_EVENT_OBJECTIVE_SECURED = 1;
	

	// Use this for initialization
	void Start () 
	{
		GameObject[] EnemySpawns = GameObject.FindGameObjectsWithTag ("EnemySpawn");
		this.totalNumObjectives = EnemySpawns.Length;

		this.gameEvent = GAME_EVENT_NONE;
	}


	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void OnLevelWasLoaded ()
	{
		
	}


	public void setPlayerWon()
	{
		this.gameEvent = GAME_EVENT_PLAYER_WON;
	}


	public void setPlayerLost()
	{
		this.gameEvent = GAME_EVENT_PLAYER_LOST;
	}


	public void clearGameStatus()
	{
		this.gameEvent = GAME_EVENT_NONE;
	}


	public void objectiveCaptured()
	{
		this.numObjectivesCaptured++;

		if (this.numObjectivesCaptured >= this.totalNumObjectives) {
			this.setPlayerWon ();
		} else {
			this.gameEvent = GAME_EVENT_OBJECTIVE_SECURED;
		}
	}


	public bool gameEventEndsGame()
	{
		switch (this.gameEvent) {

			case GAME_EVENT_PLAYER_WON:
			case GAME_EVENT_PLAYER_LOST:
				return true;
			default:
				return false;
		}
	}


	public void setStatusUpdateLocation(Vector3 UpdateLocation)
	{
		this.StatusUpdateLocation = UpdateLocation;
	}
	
	
	public Vector3 getStatusUpdateLocation()
	{
		return this.StatusUpdateLocation;
	}
	
	
	public string[] getPossibleNames()
	{
		if (this._possibleNames != null) {
			return this._possibleNames;	
		}
		
		//Generate list of names
		string nameList = this.Names.text;
		StringReader Reader = new StringReader(nameList);
		
	    string line;
		string namesSeparatedByCommas = "";
	    while ((line = Reader.ReadLine()) != null)
	    {
	        namesSeparatedByCommas += line + ",";
	    }
		//Debug.Log (namesSeparatedByCommas);

		this._possibleNames = namesSeparatedByCommas.Split(',');
		
		return this._possibleNames;
	}
	
	
	public int getTotalNumObjectives()
	{
		return this.totalNumObjectives;	
	}
	
	
	public void updateNumSoldiers(int addNumSoldiers)
	{
		this.numSoldiers += addNumSoldiers;
	}
	
	
	public int getNumSoldiers()
	{
		return this.numSoldiers;	
	}
	
	
	public void updateNumSoldiersKilled(int addNumSoldiersKilled)
	{
		this.numSoldiersKilled += addNumSoldiersKilled;	
	}
	
	
	public int getNumSoldiersKilled()
	{
		return this.numSoldiersKilled;	
	}
	
	
	public void updateNumEnemies(int addNumEnemies)
	{
		this.numEnemiesRemaining += addNumEnemies;
	}
	
	
	public int getNumEnemies()
	{
		return this.numEnemiesRemaining;	
	}
	
	
	public void updateNumEnemiesKilled(int addNumKilled)
	{
		this.numEnemiesKilled += addNumKilled;
	}
	
	
	public int getNumEnemiesKilled()
	{
		return this.numEnemiesKilled;
	}
	

}
