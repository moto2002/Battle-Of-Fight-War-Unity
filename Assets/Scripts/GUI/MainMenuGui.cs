using UnityEngine;
using System.Collections;

public class MainMenuGui : CommonGui 
{

	public GUISkin CustomGUISkin = null;
	

	// Use this for initialization
	void Start () 
	{
		this._initCommonGui();

		CommonMenuUtilities.forceMainMenuDimensions();
		//Debug.Log (Screen.width + "," + Screen.height);
		//Debug.Log (this._mainMenuWidth + "," + this._mainMenuHeight);

		//Check if there's existing persistent info, just in case. So we don't have two
		//This would happen when restarting the game
		GameObject ExistingPersistentInfo = GameObject.Find("PersistentInfo");
		if (ExistingPersistentInfo != null) {
			Destroy (ExistingPersistentInfo);
		}

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
		this.GuiAudioSource.PlayOneShot(this.Click1);

		//Here we create our first and LevelInfo object.
		GameObject PersistentInfoObj = Instantiate(Resources.Load("Prefabs/PersistentInfo")) as GameObject;
		PersistentInfoObj.name = "PersistentInfo";
		
		PersistentInfo PersistentInformation = PersistentInfoObj.GetComponent<PersistentInfo>();
		
		PersistentInformation.loadSceneAtIndex(0);
		
		//Debug.Log(sceneInfo[0]);
		//Debug.Log(sceneInfo[1]);
		
		//Application.LoadLevel ("TestMap");
	}


}




