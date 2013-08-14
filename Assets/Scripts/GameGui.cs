using UnityEngine;
using System.Collections;

public class GameGui : MonoBehaviour 
{


	public GameObject SelectedUnit = null;

	private float _generalStatsWidth = Screen.width * .19f;
	private float _generalStatsHeight = Screen.height * .19f;

	private float _squadBoxWidth = Screen.width * .25f;
	private float _squadBoxHeight = Screen.height * .25f;

	//We'll treat each second as an in-game minute
	//Start the game at 8 AM just because
	private int _startTime = 480;

	public GUISkin Skin = null;
	public Texture2D Moon;
	public Texture2D Sun;


	//General stats
	public int numObjectivesCaptured = 0;
	public int numObjectives = 0;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void OnGUI ()
	{
		GUI.skin = this.Skin;

		GUI.BeginGroup (
			new Rect (0, 0, this._generalStatsWidth, this._generalStatsHeight)
		);
		GUI.Box (new Rect (0, 0, this._generalStatsWidth, this._generalStatsHeight), "");

		int startX = (int)(this._generalStatsWidth * 0.10f);
		int startY = (int)(this._generalStatsHeight * 0.10f);
		int infoSize = (int)(this._generalStatsWidth * 0.30f);

		int currentTime = (int)(this._startTime + Time.fixedTime);
		GUI.Label (new Rect (startX, startY, infoSize, infoSize), "Day " + ((currentTime / 1440) + 1));

		startX = (int)(this._generalStatsWidth * 0.10f);
		startY = (int)(this._generalStatsHeight * 0.50f);

		int hour = currentTime / 60;
		int minute = currentTime % 60;

		GUI.Label (new Rect (startX, startY, 32, 32), this.Sun);
		GUI.Label (new Rect (startX + 32, startY, infoSize, infoSize), hour + ":" + minute.ToString("D2"));
		//GUI.Label (new Rect (startX + (int)(infoSize * 1.5), startY, infoSize, infoSize), this.numObjectivesCaptured + "/" + this.numObjectives);

		GUI.Label (new Rect (startX + 32 + (int)(infoSize * 1.5), startY, infoSize, infoSize), this.numObjectivesCaptured + "/" + this.numObjectives);

		GUI.EndGroup();

		if (this.SelectedUnit != null) {
			//Group wrapper helps collect UI controls together
			GUI.BeginGroup (
				new Rect (0, Screen.height - this._squadBoxHeight, this._squadBoxWidth, this._squadBoxHeight)
				);
			GUI.Box (new Rect (0, 0, this._squadBoxWidth, this._squadBoxHeight), "Squad Details");		
			GUI.Label (new Rect (10, 40, 80, 20), "Restart!");
			GUI.Label (new Rect (10, 80, 80, 20), "Options!");

			GUI.EndGroup();
		}
	}
}
