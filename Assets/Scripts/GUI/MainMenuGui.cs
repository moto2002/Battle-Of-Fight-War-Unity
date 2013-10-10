using UnityEngine;
using System.Collections;

public class MainMenuGui : MonoBehaviour 
{

	public GUISkin CustomGUISkin = null;

	private float _mainMenuWidth = Screen.width * 0.35f;
	private float _mainMenuHeight = Screen.height * 0.50f;


	// Use this for initialization
	void Start () 
	{
		//Debug.Log (Screen.width + "," + Screen.height);
		//Debug.Log (this._mainMenuWidth + "," + this._mainMenuHeight);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}


	void OnGUI()
	{
		GUI.skin = this.CustomGUISkin;

		GUILayout.BeginArea (
			new Rect (
				(Screen.width * 0.50f) - (this._mainMenuWidth * 0.50f), 
				(Screen.height * 0.50f) - (this._mainMenuHeight * 0.50f), 
				this._mainMenuWidth, 
				this._mainMenuHeight)
			);
		GUILayout.BeginVertical ("", GUI.skin.box);


		GUIStyle LabelStyle = this.CustomGUISkin.GetStyle ("Label");
		int originalFontSize = LabelStyle.fontSize;

		LabelStyle.fontSize = 12;

		GUILayout.Space (20);
		GUILayout.Label ("Main Menu");

		LabelStyle.fontSize = originalFontSize;

		GUILayout.FlexibleSpace ();
		CommonMenuUtilities.drawButton ("Start Game", startGame);

		CommonMenuUtilities.drawCommonMainMenuItems ();

		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical ();
		GUILayout.EndArea();
	}


	public delegate void buttonResponse();


	public void startGame()
	{
		Application.LoadLevel ("TestMap");
	}


}




