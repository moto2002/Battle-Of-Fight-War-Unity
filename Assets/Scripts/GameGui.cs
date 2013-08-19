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

	public GUISkin CustomGUISkin = null;
	public Texture2D Moon;
	public Texture2D Sun;
	public Texture2D Flag;


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
		GUI.skin = this.CustomGUISkin;

		GUILayout.BeginArea (new Rect (0, 0, this._generalStatsWidth, this._generalStatsHeight));
		GUILayout.BeginVertical ("", GUI.skin.box);

		GUILayout.FlexibleSpace ();

		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		int currentTime = (int)(this._startTime + Time.fixedTime);
		GUILayout.Label ("Day " + ((currentTime / 1440) + 1));

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		int hour = currentTime / 60;
		int minute = currentTime % 60;

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();

		GUILayout.Label (this.Sun);
		GUILayout.Label (hour + ":" + minute.ToString("D2"));

		GUILayout.FlexibleSpace ();
		//GUI.Label (new Rect (startX + (int)(infoSize * 1.5), startY, infoSize, infoSize), this.numObjectivesCaptured + "/" + this.numObjectives);

		GUILayout.Label (this.Flag);
		GUILayout.Label (this.numObjectivesCaptured + "/" + this.numObjectives);

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		GUILayout.EndVertical ();
		GUILayout.EndArea();

		if (this.SelectedUnit != null) {
			//Group wrapper helps collect UI controls together
			GUILayout.BeginArea(new Rect (0, Screen.height - this._squadBoxHeight, this._squadBoxWidth, this._squadBoxHeight));

			GUILayout.BeginVertical ("", GUI.skin.box);
			GUILayout.Label ("Squad Details");

			GUILayout.FlexibleSpace ();

			Unit UnitDetails = this.SelectedUnit.GetComponent<Unit> ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			foreach (SquadMember Squaddie in UnitDetails.SquadMembers) {
				GUILayout.Label (Squaddie.name);
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();

			GUILayout.FlexibleSpace ();
			GUILayout.EndVertical ();

			GUILayout.EndArea();
		}
	}
}
