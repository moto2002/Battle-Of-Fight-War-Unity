using UnityEngine;
using System.Collections;

public class UnitBehavior : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void OnMouseEnter()
	{
		//Use sprite manager to get texture, then set this guy's sprite

		/**
		Texture NewTexture = new Texture(64, 64, TextureFormat.ARGB32, true);

		Color[] NewMainTexPixels = new Color[64 * 64];
		Color DarkYellow = new Color (200.0f/255.0f, 175.0f / 255.0f, 35.0f / 255.0f);
		for (int i = 0; i < 64; i++) {
			for (int j = 0; j < 64; j++) {
				Color PixelColor = this._OriginalTexture.GetPixel(i, j);

				int pixelPosition = 64 * j + i;
				if (PixelColor.b > (200.0f/255.0f)) {
					NewMainTexPixels [pixelPosition] = Color.yellow;
				} else if (PixelColor.b > (100.0f/255.0f)) { //Darker color
					NewMainTexPixels [pixelPosition] = DarkYellow;
				}
			}
		}

		NewTexture.SetPixels (NewMainTexPixels);
		NewTexture.Apply();
		this.renderer.material.mainTexture = NewTexture; */
	}


	void OnMouseExit()
	{
	}

}
