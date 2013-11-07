using UnityEngine;
using System.Collections;

public class LeveiIntroGui : MonoBehaviour 
{
	
	public GUISkin CustomGUISkin = null;

	float _boxWidth = Screen.width * 0.75f;
	float _boxHeight = Screen.height * 0.80f;
	
	float _maxWidth = 650.0f;
	float _maxHeight = 500.0f;
	

	// Use this for initialization
	void Start () 
	{
		this._boxWidth = CommonMenuUtilities.forceDimensions(this._boxWidth, this._maxWidth, this._maxWidth);
		this._boxHeight = CommonMenuUtilities.forceDimensions(this._boxHeight, this._maxHeight, this._maxHeight);
		
		Debug.Log("Briefing width: " + this._boxWidth);
		Debug.Log("Briefing height: " + this._boxHeight);
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
		
		if (GUILayout.Button("Continue")) {
			this._loadNextScene();
		}
		
		GUILayout.FlexibleSpace();
		
		CommonMenuUtilities.endCenterBox();
	}
	
	
	private void _drawPicture()
	{
		CommonMenuUtilities.drawSingleWrappingLabelLine("PICTURE GOES HERE");
	}
	
	
	private void _drawText()
	{
		CommonMenuUtilities.drawSingleWrappingLabelLine(
			"Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut " +
			"laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper " +
			"suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in " +
			"vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan " +
			"et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. "
		);
	}
	
	
	private void _loadNextScene()
	{
		
	}
}
