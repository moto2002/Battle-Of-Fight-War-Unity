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
	//Start the game at 8 AM just because
	private int _startTime = 480;

	private int _statusTextFontSize = 16;

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
		//If game is paused
		if (Time.timeScale == 0) {

			//User hit esc during pause
			if (Input.GetKeyDown (KeyCode.Escape)) {

				//Game-ending event? Then let's go to the post-game stats page
				if (this.LevelInformation.gameEventEndsGame ()) {


				} else { //It was just a normal status update... resume game
					Time.timeScale = 1;
					this.LevelInformation.gameEvent = LevelInfo.GAME_EVENT_NONE;
					this.LevelInformation.StatusUpdateLocation = new Vector3 (-1.0f, -1.0f, -1.0f);
					this.Camera.setFocusedOnEvent (false);
					this.Camera.setForcedMove (false);
				}

			}
		}
	
	}


	void OnGUI ()
	{
		GUI.skin = this.CustomGUISkin;

		this.drawGeneralStatsBox ();

		if (this.LevelInformation.gameEvent > LevelInfo.GAME_EVENT_NONE) {

			if (!this.Camera.isForcedMove()) {

				this.Camera.setForcedMove (true);
				this.Camera.setFocusPosition (this.LevelInformation.StatusUpdateLocation);

				//Pause the game
				//Setting timeScale to 0 basically disables any framerate-independent shit (FixedUpdate, Time.deltaTime, etc)
				Time.timeScale = 0;
			}

			if (!this.Camera.isFocusedOnEvent()) {
				return;
			}

			switch (this.LevelInformation.gameEvent) {

				case LevelInfo.GAME_EVENT_PLAYER_WON:
					this.drawPlayerWonBox ();
					break;
				case LevelInfo.GAME_EVENT_PLAYER_LOST:
					this.drawPlayerLostBox ();
					break;

				default: //Not a game-ending event

					switch (this.LevelInformation.gameEvent) {

						case LevelInfo.GAME_EVENT_OBJECTIVE_SECURED:
							int numLeft = LevelInformation.totalNumObjectives - LevelInformation.numObjectivesCaptured;
							this.drawStatusBox ("Your forces secured an objective\nYou have " + numLeft + " objectives remaining");
							break;
						default:
							break;
					}

					break;
			}

			return;
		} 

		if (this.SelectedUnit != null) {
			this.drawSquadBox ();
		}
	}


	void drawGeneralStatsBox()
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
		GUILayout.Label (LevelInformation.numObjectivesCaptured + "/" + LevelInformation.totalNumObjectives);

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		GUILayout.EndVertical ();
		GUILayout.EndArea();
	}


	void drawSquadBox()
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


	void drawPlayerWonBox()
	{
		this.drawStatusBox ("All objectives have been secured\nYour forces are victorious");
	}


	void drawPlayerLostBox()
	{
		this.drawStatusBox ("Your base has been captured\nYour forces have been defeated");
	}


	void drawStatusBox(string message)
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


}
