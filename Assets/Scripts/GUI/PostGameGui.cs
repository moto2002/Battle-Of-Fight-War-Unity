using UnityEngine;
using System.Collections;

public class PostGameGui : MonoBehaviour 
{
	
	public GUISkin CustomGUISkin = null;
	public BattleInfo BattleInformation;
	
	float _boxWidth = Screen.width * 0.5f;
	float _boxHeight = Screen.height * 0.7f;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void OnGUI()
	{
		GUI.skin = this.CustomGUISkin;

		CommonMenuUtilities.drawCenterBoxHeader(this.CustomGUISkin, "BATTLE REPORT", this._boxWidth, this._boxHeight);
		
		CommonMenuUtilities.drawHorizonalLabel("# of Days");
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.drawHorizonalLabel("Soldiers Remaining");
		CommonMenuUtilities.drawHorizonalLabel("Soldiers Lost");
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.drawHorizonalLabel("Enemies Remaining");
		CommonMenuUtilities.drawHorizonalLabel("Enemies Killed");
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.endCenterBox();
	}
}
