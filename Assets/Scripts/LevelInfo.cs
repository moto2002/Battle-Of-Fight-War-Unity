using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class LevelInfo : MonoBehaviour 
{

	public int totalNumObjectives = 0;
	public int gameEvent = GAME_EVENT_NONE;
	
	public int numObjectivesCaptured = 0;
	
	public int numSoldiers = 0;
	public int numSoldiersKilled = 0;
	
	public int numEnemiesRemaining = 0;
	public int numEnemiesKilled = 0;
	
	public int battleTime = 0;
	
	//To record battle time (in seconds)
	private int _startTime = 0;
	private int _timeSinceSceneLoaded = 0;

	public Vector3 StatusUpdateLocation;

	public TextAsset Names;
	private string[] _possibleNames;	
	

	public const int GAME_EVENT_NONE = 0;
	public const int GAME_EVENT_PAUSED = 1;
	public const int GAME_EVENT_PLAYER_WON = 2;
	public const int GAME_EVENT_PLAYER_LOST = 3;

	public const int GAME_EVENT_OBJECTIVE_SECURED = 1;
	
	public const int TIME_DAY_THRESHOLD = 390;
	public const int TIME_NIGHT_THRESHOLD = 1170;
	

	private const int TIME_MODIFIER = 3;

	// Use this for initialization
	void Start () 
	{
		if (Application.loadedLevelName != "PostGameStats") { //If this is post-game-stats, don't do shit
			DontDestroyOnLoad(this.gameObject);
		}
		
		//We can immediately unset the parent since I only needed that relationship for easy map-prefab-making
		this.transform.parent = null;

		//Putting this time shit deliberately in Start() so it's "closer" to the actual start of the game
		//(As opposed to, say, Awake(), which is called before Start()
		this._startTime = 300; //Battles start at early just because that's what time people wake up and do shit
		this._timeSinceSceneLoaded = (int)(Time.fixedTime) * TIME_MODIFIER;
	}
	
	
	//Using this because we don't want to zero-reset records of shit after everything's all initialized on the map
	//Thus we reset records before any other objects' Start() stuff is called
	void Awake()
	{
		//Reset all non-zero "record" statistics to start just to be sure.
		
		GameObject[] EnemySpawns = GameObject.FindGameObjectsWithTag ("EnemySpawn");
		this.totalNumObjectives = EnemySpawns.Length;
		
		if (Application.loadedLevelName != "PostGameStats") { //If this is post-game-stats, don't do shit
			DontDestroyOnLoad(this.gameObject);
			Debug.Log("WILL NOT DeSTROY LEVEL INFO");
		}
		
	}


	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void FixedUpdate()
	{
		this.battleTime = (int)(Time.fixedTime * TIME_MODIFIER) + this._startTime - this._timeSinceSceneLoaded;	
	}
	
	
	//Note that this is only triggered by Application.LoadLevel
	//The order is OnLevelWasLoaded, Awake, Start
	void OnLevelWasLoaded ()
	{
		/**
		if (Application.loadedLevel != 1) { //If this is post-game-stats, don't do shit
			DontDestroyOnLoad(this.gameObject);
		}
		*/
	}


	public void setPlayerWon()
	{
		//In case the game's already over
		if (this.gameEvent == GAME_EVENT_PLAYER_LOST) {
			return;
		}
		this.gameEvent = GAME_EVENT_PLAYER_WON;
	}


	public void setPlayerLost()
	{
		//In case the game's already over
		if (this.gameEvent == GAME_EVENT_PLAYER_WON) {
			return;
		}
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
	
	
	public int getBattleTime()
	{
		return this.battleTime;	
	}

}
