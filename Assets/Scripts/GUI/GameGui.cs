using UnityEngine;
using System.Collections;

public class GameGui : MonoBehaviour 
{


	public GameObject SelectedUnit = null;


	private float _generalStatsWidth = Screen.width * .19f;
	private float _generalStatsHeight = Screen.height * .19f;

	private float _gameEventWidth = Screen.width * .33f;
	private float _gameEventHeight = Screen.height * .33f;

	private float _squadBoxWidth = Screen.width * .30f;
	private float _squadBoxHeight = Screen.height * .50f;

	//We'll treat each second as an in-game minute
	//Start the game at 8 AM just because that's what time people work and shit or something
	private int _startTime = 480;
	private int _statusTextFontSize = 16;
	private bool _showPauseMenu = false;
	

	public GUISkin CustomGUISkin = null;

	//Icons
	public Texture2D Moon;
	public Texture2D Sun;
	public Texture2D Flag;
	public Texture2D TimeOfDay = null;

	public LevelInfo LevelInformation;
	public CameraMovement Camera;

	// Use this for initialization
	void Start () 
	{
		this.TimeOfDay = this.Sun;

		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		this.LevelInformation = LevelInfoObj.GetComponent<LevelInfo>();

		GameObject CameraObj = GameObject.Find ("MainCamera");
		this.Camera = CameraObj.GetComponent<CameraMovement> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If game is paused and camera is moving on its own
		//This should take precedence over every other GUI thingy since it's an important event	
		if (Time.timeScale == 0 && this.Camera.isForcedMove()) {
			
			//Don't let the user escape out of this until the camera is focused
			//(and thus the corresponding event window info shows up)
			if (!this.Camera.isFocusedOnEvent()) {
				return;
			}
			
			//User hit esc during pause
			if (Input.GetKeyDown (KeyCode.Escape)) {

				//Game-ending event? Then let's go to the post-game stats page
				if (this.LevelInformation.gameEventEndsGame ()) {
					Application.LoadLevel("PostGameStats");
				} else { //It was just a normal status update... resume game as normal
					Time.timeScale = 1;
					this.LevelInformation.gameEvent = LevelInfo.GAME_EVENT_NONE;
					this.LevelInformation.setStatusUpdateLocation(new Vector3 (-1.0f, -1.0f, -1.0f));
					this.Camera.setFocusedOnEvent (false);
					this.Camera.setForcedMove (false);
				}
				
			}
			
			return;
		}
		
		//Game is not paused; no other special circumstances
		if (Time.timeScale > 0 && Input.GetKeyDown(KeyCode.Escape)) {
			
			this._showPauseMenu = !this._showPauseMenu;
			if (this._showPauseMenu) {
				Time.timeScale = 0;	
			} else {
				Time.timeScale = 1;	
			}
		}
	}


	void OnGUI ()
	{
		GUI.skin = this.CustomGUISkin;
		
		//Always show the general stats box
		this._drawGeneralStatsBox ();
		
		//Check if there's a game event going on
		if (this.LevelInformation.gameEvent > LevelInfo.GAME_EVENT_NONE) {
			
			//We have an event, move the camera to focus on said event
			if (!this.Camera.isForcedMove()) {

				this.Camera.setForcedMove (true);
				this.Camera.setFocusPosition (this.LevelInformation.getStatusUpdateLocation());

				//Pause the game
				//Setting timeScale to 0 basically disables any framerate-independent shit (FixedUpdate, Time.deltaTime, etc)
				Time.timeScale = 0;
			}
			
			//Is the camera still moving on its own?
			if (!this.Camera.isFocusedOnEvent()) {
				return;
			}

			switch (this.LevelInformation.gameEvent) {

				case LevelInfo.GAME_EVENT_PLAYER_WON:
					this._drawPlayerWonBox ();
					break;
				case LevelInfo.GAME_EVENT_PLAYER_LOST:
					this._drawPlayerLostBox ();
					break;

				default: //Not a game-ending event

					switch (this.LevelInformation.gameEvent) {

						case LevelInfo.GAME_EVENT_OBJECTIVE_SECURED:
							int numLeft = LevelInformation.getTotalNumObjectives() - LevelInformation.numObjectivesCaptured;
							this._drawStatusBox ("Your forces secured an objective\nYou have " + numLeft + " objectives remaining");
							break;
						default:
							break;
					}

					break;
			}

			return;
		} 

		if (this.SelectedUnit != null) {
			this._drawSquadBox ();
		}
		
		if (this._showPauseMenu) {
			this._drawPauseMenu();			
		}
	}


	private void _drawGeneralStatsBox()
	{
		GUILayout.BeginArea (new Rect (0, 0, this._generalStatsWidth, this._generalStatsHeight));
		GUILayout.BeginVertical ("", GUI.skin.box);

		GUILayout.FlexibleSpace ();

		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		int currentTime = (int)(this._startTime + Time.fixedTime);
		GUILayout.Label ("Day " + ((currentTime / 1440) + 1));

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		int hour = currentTime / 60;
		int minute = currentTime % 60;

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();

		GUILayout.Label (this.TimeOfDay);
		GUILayout.Label (hour + ":" + minute.ToString("D2"));

		GUILayout.FlexibleSpace ();
		//GUI.Label (new Rect (startX + (int)(infoSize * 1.5), startY, infoSize, infoSize), this.numObjectivesCaptured + "/" + this.numObjectives);

		GUILayout.Label (this.Flag);
		GUILayout.Label (LevelInformation.numObjectivesCaptured + "/" + LevelInformation.getTotalNumObjectives());

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		GUILayout.EndVertical ();
		GUILayout.EndArea();
	}


	private void _drawSquadBox()
	{
		Unit UnitDetails = this.SelectedUnit.GetComponent<Unit> ();

		//Group wrapper helps collect UI controls together
		GUILayout.BeginArea(new Rect (0, Screen.height - this._squadBoxHeight, this._squadBoxWidth, this._squadBoxHeight));

		GUILayout.BeginVertical ("", GUI.skin.box);
		GUILayout.Space (10);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Squad Details");
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label (UnitDetails.currentAction);
		GUILayout.EndHorizontal ();

		GUILayout.Space (10);
		

		int count = 0;
		foreach (SquadMember Squaddie in UnitDetails.SquadMembers) {
			count++;
			if (count % 3 == 1) {
				GUILayout.BeginHorizontal ();
				//GUILayout.Space (10);
				GUILayout.FlexibleSpace ();
			}

			GUILayout.BeginVertical ();
			GUILayout.Label (Squaddie.name);

			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();

			Color OriginalBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.green;

			float healthWidth = (Squaddie.health / 100.0f) * 45.0f; //I decided 45 px to be a good size for health bar
			GUILayout.Label ("", GUI.skin.GetStyle("HealthBar"), GUILayout.Width(healthWidth));
			GUI.backgroundColor = OriginalBackgroundColor;

			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();

			GUILayout.Label (Squaddie.SquadViewTexture);
			GUILayout.EndVertical ();

			GUILayout.FlexibleSpace ();
			if (count % 3 == 0 || count == UnitDetails.SquadMembers.Count) {
				GUILayout.EndHorizontal ();
				GUILayout.Space (15);
			}
		}

		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical ();

		GUILayout.EndArea();
	}


	private void _drawPlayerWonBox()
	{
		this._drawStatusBox ("All objectives have been secured\nYour forces are victorious");
	}


	private void _drawPlayerLostBox()
	{
		this._drawStatusBox ("Your base has been captured\nYour forces have been defeated");
	}


	private void _drawStatusBox(string message)
	{
		//GUIStyle LabelStyle = this.CustomGUISkin.GetStyle ("Label");
		//int originalFontSize = LabelStyle.fontSize;

		//LabelStyle.fontSize = this._statusTextFontSize;

		GUILayout.BeginArea (new Rect ((Screen.width * 0.5f) - (this._gameEventWidth * 0.5f), (Screen.height * 0.5f) - (this._gameEventWidth * 0.5f), this._gameEventWidth, this._gameEventHeight));
		GUILayout.BeginVertical ("", GUI.skin.box);
		GUILayout.FlexibleSpace ();

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();

		GUILayout.Label (message);

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical ();
		GUILayout.EndArea();
		
		//LabelStyle.fontSize = originalFontSize;
	}
	
	
	private void _drawPauseMenu()
	{
		if (CommonMenuUtilities.showOptions) {
			CommonMenuUtilities.drawOptionsMenu(GUI.skin);
			return;
		}
		
		CommonMenuUtilities.drawMainMenuHeader(GUI.skin);
		
		CommonMenuUtilities.drawButton ("Resume Game", unpauseGame);

		CommonMenuUtilities.drawCommonMainMenuItems ();

		CommonMenuUtilities.endCenterBox();
	}
	
	
	public void unpauseGame()
	{
		this._showPauseMenu = false;
		Time.timeScale = 1;
	}


}