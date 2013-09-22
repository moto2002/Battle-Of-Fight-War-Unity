using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour 
{

	public int minSpawnsFromEnemySpawner;
	public int maxSpawnsFromEnemySpawner;
	public int timeBetweenSpawns;

	public int numObjectivesCaptured;
	public int totalNumObjectives;
	public int gameStatus;

	public TextAsset Names;

	public const int GAME_STATUS_RUNNING = 0;
	public const int GAME_STATUS_PAUSED = 1;
	public const int GAME_STATUS_PLAYER_WON = 2;
	public const int GAME_STATUS_PLAYER_LOST = 3;

	// Use this for initialization
	void Start () 
	{
		GameObject[] EnemySpawns = GameObject.FindGameObjectsWithTag ("EnemySpawn");
		this.totalNumObjectives = EnemySpawns.Length;

		this.gameStatus = GAME_STATUS_RUNNING;
	}


	// Update is called once per frame
	void Update () 
	{
	
	}


	public void setPlayerWon()
	{
		this.gameStatus = GAME_STATUS_PLAYER_WON;
	}


	public void setPlayerLost()
	{
		this.gameStatus = GAME_STATUS_PLAYER_LOST;
	}
}
