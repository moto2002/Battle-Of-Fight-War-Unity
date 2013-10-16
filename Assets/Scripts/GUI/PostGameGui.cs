using UnityEngine;
using System.Collections;

public class PostGameGui : MonoBehaviour 
{
	
	public GUISkin CustomGUISkin = null;

	float _boxWidth = Screen.width * 0.5f;
	float _boxHeight = Screen.height * 0.7f;
	
	private LevelInfo _LevelInformations;
	
	// Use this for initialization
	void Start () 
	{
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		if (LevelInfoObj != null) {
			this._LevelInformations = LevelInfoObj.GetComponent<LevelInfo>();
		} else {
			//For debugging really
			this._LevelInformations = new LevelInfo();
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void OnGUI()
	{
		GUI.skin = this.CustomGUISkin;

		CommonMenuUtilities.drawCenterBoxHeader(this.CustomGUISkin, "BATTLE REPORT", this._boxWidth, this._boxHeight);
		
		CommonMenuUtilities.drawSingleLabelLine("# of Days Fought\t\t" + this._LevelInformations.getBattleEndTime());
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.drawSingleLabelLine("Soldiers Remaining\t\t" + this._LevelInformations.getNumSoldiers());
		CommonMenuUtilities.drawSingleLabelLine("Soldiers Lost\t\t" + this._LevelInformations.getNumSoldiersKilled());
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.drawSingleLabelLine("Enemies Remaining\t\t" + this._LevelInformations.getNumEnemies());
		CommonMenuUtilities.drawSingleLabelLine("Enemies Killed\t\t" + this._LevelInformations.getNumEnemiesKilled());
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.endCenterBox();
	}
}
