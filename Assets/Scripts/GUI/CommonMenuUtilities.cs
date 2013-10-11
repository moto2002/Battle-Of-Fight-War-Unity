using UnityEngine;
using System.Collections;

public class CommonMenuUtilities
{

	public static bool showOptions = false;
	
	private static float _mainMenuWidth = Screen.width * 0.35f;
	private static float _mainMenuHeight = Screen.height * 0.50f;


	public delegate void buttonResponse();



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
	
	
	public static void drawMainMenuHeader(GUISkin CustomGuiSkin, string menuHeading = "Main Menu")
	{
		GUILayout.BeginArea (
			new Rect (
				(Screen.width * 0.50f) - (CommonMenuUtilities._mainMenuWidth * 0.50f), 
				(Screen.height * 0.50f) - (CommonMenuUtilities._mainMenuHeight * 0.50f), 
				CommonMenuUtilities._mainMenuWidth, 
				CommonMenuUtilities._mainMenuHeight)
			);
		GUILayout.BeginVertical ("", GUI.skin.box);
		
		GUIStyle LabelStyle = CustomGuiSkin.GetStyle ("Label");
		int originalFontSize = LabelStyle.fontSize;

		LabelStyle.fontSize = 12;

		GUILayout.Space (20);
		GUILayout.Label (menuHeading);

		LabelStyle.fontSize = originalFontSize;

		GUILayout.FlexibleSpace ();
	}
	
	
	public static void endMainMenu()
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
		CommonMenuUtilities.drawMainMenuHeader(CustomGuiSkin, "Options");
		
		CommonMenuUtilities.drawButton("Back", exitOptions);
		
		CommonMenuUtilities.endMainMenu();
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


}
