using UnityEngine;
using System.Collections;

public class CommonMenuUtilities
{

	public static bool showOptions = false;
	
	private static float _mainMenuWidth = Screen.width * 0.35f;
	private static float _mainMenuHeight = Screen.height * 0.50f;
	
	private static float _mainMenuMinWidth = 310.0f;
	private static float _mainMenuMinHeight = 250.0f;
	
	private static float _mainMenuMaxWidth = 426.0f;
	private static float _mainMenuMaxHeight = 316.0f;


	public delegate void buttonResponse();

	
	public static void forceMainMenuDimensions()
	{
		_mainMenuWidth = forceDimensions(_mainMenuWidth, _mainMenuMinWidth, _mainMenuMaxWidth);
		_mainMenuHeight = forceDimensions(_mainMenuHeight, _mainMenuMinHeight, _mainMenuMaxHeight);
		
		//Debug.Log ("Main Menu width:" + _mainMenuWidth);
		//Debug.Log ("Main Menu height:" + _mainMenuHeight);
	}
	

	public static void displayOptions()
	{
		CommonMenuUtilities.showOptions = true;
	}
	
	
	public static void exitOptions()
	{
		CommonMenuUtilities.showOptions = false;	
	}


	public static void exitGame()
	{
		Debug.Log ("Quitting");
		Application.Quit ();
	}
	
	
	public static void drawMainMenuHeader(GUISkin CustomGuiSkin, string menuHeading = "MAIN MENU")
	{
		CommonMenuUtilities.drawCenterBoxHeader(CustomGuiSkin, menuHeading, CommonMenuUtilities._mainMenuWidth, CommonMenuUtilities._mainMenuHeight);
	}
	
	
	public static void drawCenterBoxHeader(GUISkin CustomGuiSkin, string menuHeading, float width, float height)
	{
		//Debug.Log ("width:" + _mainMenuWidth);
		//Debug.Log ("height: " + _mainMenuHeight);
		
		GUILayout.BeginArea (
			new Rect (
				(Screen.width * 0.50f) - (width * 0.50f), 
				(Screen.height * 0.50f) - (height * 0.50f), 
				width, 
				height)
			);
		GUILayout.BeginVertical ("", GUI.skin.box);

		if (menuHeading == "") {
			return;
		}

		GUIStyle LabelStyle = CustomGuiSkin.GetStyle ("Label");
		int originalFontSize = LabelStyle.fontSize;

		LabelStyle.fontSize = 12;

		GUILayout.Space (20);
		
		GUILayout.Label (menuHeading);

		LabelStyle.fontSize = originalFontSize;

		GUILayout.FlexibleSpace ();
	}
	
	
	public static void endCenterBox()
	{
		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical ();
		GUILayout.EndArea();
	}


	public static void drawCommonMainMenuItems()
	{
		GUILayout.Space (20);
		CommonMenuUtilities.drawButton ("Options", CommonMenuUtilities.displayOptions);

		GUILayout.Space (20);
		CommonMenuUtilities.drawButton ("Exit Game", CommonMenuUtilities.exitGame);
	}

	
	public static void drawOptionsMenu(GUISkin CustomGuiSkin)
	{
		CommonMenuUtilities.drawCenterBoxHeader(CustomGuiSkin, "OPTIONS", CommonMenuUtilities._mainMenuWidth, CommonMenuUtilities._mainMenuHeight);
		
		CommonMenuUtilities.drawButton("Back", exitOptions);
		
		CommonMenuUtilities.endCenterBox();
	}
	
	
	public static void drawButton(string buttonText, buttonResponse buttonResponseFunction)
	{
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button (buttonText)) {
			buttonResponseFunction ();
		};
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
	}
	
	
	public static void drawSingleLabelLine(string labelText)
	{	
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace ();
		GUILayout.Label(labelText);
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
	}
	
	
	public static void drawSingleWrappingLabelLine(string labelText)
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace ();
		GUILayout.Label(labelText, GUI.skin.GetStyle("WordWrapLabel"));
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
	}
	
	
	public static float forceDimensions(float actualParam, float minParam, float maxParam)
	{
		if (actualParam < minParam) {
			return minParam;
		}
		
		if (actualParam > maxParam) {
			return maxParam;	
		}
		
		return actualParam;
	}

}
