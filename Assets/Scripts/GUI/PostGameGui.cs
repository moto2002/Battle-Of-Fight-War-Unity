using UnityEngine;
using System.Collections;

public class PostGameGui : CommonGui 
{
	
	public GUISkin CustomGUISkin = null;

	float _boxWidth = Screen.width * 0.5f;
	float _boxHeight = Screen.height * 0.7f;
	
	private float _maxWidth = 610.0f;
	private float _maxHeight = 440.0f;

	private string _title = "";

	private LevelInfo _LevelInformations;

	public delegate void ContinueButtonResponse();
	
	private ContinueButtonResponse _ContinueResponseFunction;
	
	// Use this for initialization
	void Start () 
	{
		this._initCommonGui();

		this._boxWidth = CommonMenuUtilities.forceDimensions(this._boxWidth, this._maxWidth, this._maxWidth);	
		this._boxHeight = CommonMenuUtilities.forceDimensions(this._boxHeight, this._maxHeight, this._maxHeight);
		
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		if (LevelInfoObj != null) {
			this._LevelInformations = LevelInfoObj.GetComponent<LevelInfo>();
		} else {
			//For debugging really
			this._LevelInformations = new LevelInfo();
		}
		
		//Technically this is all paused
		Time.timeScale = 0;	
		
		//Debug.Log("PostGame width: " + this._boxWidth);
		//Debug.Log("PostGame height: " + this._boxHeight);

		if (this._LevelInformations.gameEvent == LevelInfo.GAME_EVENT_PLAYER_WON) {
			this._title = "YOU ARE VICTORIOUS";
			this._ContinueResponseFunction = _loadNextScene;
		} else {
			this._title = "YOU HAVE BEEN DEFEATED";
			this._ContinueResponseFunction = _loadDefeatScene;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void OnGUI()
	{
		GUI.skin = this.CustomGUISkin;

		CommonMenuUtilities.drawCenterBoxHeader(this.CustomGUISkin, this._title, this._boxWidth, this._boxHeight);
		
		CommonMenuUtilities.drawSingleLabelLine("BATTLE REPORT");

		GUILayout.FlexibleSpace();

		CommonMenuUtilities.drawSingleLabelLine("Days Fought\t\t" + (this._LevelInformations.getBattleTime() / 1440) + 1);
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.drawSingleLabelLine("Soldiers Remaining\t\t" + this._LevelInformations.getNumSoldiers());
		CommonMenuUtilities.drawSingleLabelLine("Soldiers Lost\t\t" + this._LevelInformations.getNumSoldiersKilled());
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.drawSingleLabelLine("Enemies Remaining\t\t" + this._LevelInformations.getNumEnemies());
		CommonMenuUtilities.drawSingleLabelLine("Enemies Killed\t\t" + this._LevelInformations.getNumEnemiesKilled());
		GUILayout.FlexibleSpace();
		
		if (GUILayout.Button("Continue")) {
			this.GuiAudioSource.PlayOneShot(this.Click1);
			this._ContinueResponseFunction();
		}
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.endCenterBox();
	}
	
	
	private void _loadNextScene()
	{
		//Make sure to destroy the LevelInfo that we carried over from the map
		GameObject LevelInfo = GameObject.Find("LevelInfo");
		Destroy(LevelInfo);
		
		PersistentInfo Persistence = GameObject.Find("PersistentInfo").GetComponent<PersistentInfo>();
		Persistence.loadNextScene();
	}

	private void _loadDefeatScene()
	{
		GameObject LevelInfo = GameObject.Find("LevelInfo");
		Destroy(LevelInfo);
		
		PersistentInfo Persistence = GameObject.Find("PersistentInfo").GetComponent<PersistentInfo>();
		Persistence.loadDefeatScene();
	}

}
