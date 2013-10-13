using UnityEngine;
using System.Collections;

public class MainMenuGui : MonoBehaviour 
{

	public GUISkin CustomGUISkin = null;
	

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
		
		if (CommonMenuUtilities.showOptions) {
			CommonMenuUtilities.drawOptionsMenu(GUI.skin);
			return;
		}
		
		CommonMenuUtilities.drawMainMenuHeader(GUI.skin);
		
		CommonMenuUtilities.drawButton ("Start Game", startGame);

		CommonMenuUtilities.drawCommonMainMenuItems ();
		
		CommonMenuUtilities.endCenterBox();
	}
	

	public void startGame()
	{
		Application.LoadLevel ("TestMap");
	}


}




