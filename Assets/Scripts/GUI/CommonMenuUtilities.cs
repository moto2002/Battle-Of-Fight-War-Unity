using UnityEngine;
using System.Collections;

public class CommonMenuUtilities
{

	static bool showOptions = false;


	public delegate void buttonResponse();


	public static void options()
	{
		CommonMenuUtilities.showOptions = !CommonMenuUtilities.showOptions;
	}


	public static void exitGame()
	{
		Debug.Log ("Quitting");
		Application.Quit ();
	}


	public static void drawCommonMainMenuItems()
	{
		GUILayout.Space (20);
		CommonMenuUtilities.drawButton ("Options", CommonMenuUtilities.options);

		GUILayout.Space (20);
		CommonMenuUtilities.drawButton ("Exit Game", CommonMenuUtilities.exitGame);
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
