using UnityEngine;
using System.Collections;

public class LeveiIntroGui : MonoBehaviour 
{
	
	public GUISkin CustomGUISkin = null;

	float _boxWidth = Screen.width * 0.75f;
	float _boxHeight = Screen.height * 0.80f;
	

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
		
		//Replace "briefing" with the name of the map or something (should be in the intro text file)
		CommonMenuUtilities.drawCenterBoxHeader(this.CustomGUISkin, "Briefing", this._boxWidth, this._boxHeight);

		GUILayout.FlexibleSpace();
		
		this._drawPicture();
		
		GUILayout.FlexibleSpace();
		
		this._drawText();

		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.endCenterBox();
	}
	
	
	private void _drawPicture()
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace ();
		GUILayout.Label(
			"PICTURE GOES HERE"
			,
			GUI.skin.GetStyle("WordWrapLabel")
		);
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
	}
	
	
	private void _drawText()
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace ();
		GUILayout.Label(
			"Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut " +
			"laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper " +
			"suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in " +
			"vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan " +
			"et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. ",
			GUI.skin.GetStyle("WordWrapLabel")
		);
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
	}
}
