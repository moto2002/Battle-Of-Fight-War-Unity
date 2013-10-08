using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour 
{

	public int minSpawnsFromEnemySpawner;
	public int maxSpawnsFromEnemySpawner;
	public int timeBetweenSpawns;

	public int numObjectivesCaptured;
	public int totalNumObjectives;
	public int gameEvent;

	public Vector3 StatusUpdateLocation;

	public TextAsset Names;

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

}
