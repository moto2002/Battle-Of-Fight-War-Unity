using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	private int _mapSize = 100;

	private const int _GRASS = 1;

	// Use this for initialization
	void Start () 
	{

		// Create a new X x Y texture ARGB32 (32 bit with alpha) and no mipmaps
		Texture2D Texture = new Texture2D(this._mapSize, this._mapSize, TextureFormat.ARGB32, false);
		Texture2D NormalMap = new Texture2D (this._mapSize, this._mapSize, TextureFormat.RGB24, false);

		/**
		 * From the internet:
		 * "Real" bump maps are greyscale heightmaps so if you've got an 8bit bump map then your values go from 0 to 255 
		 * where 0 is the "deepest" value and 255 is the "highest" value.
		 * 
		 * For more on Unity bumpmap colors:
		 * http://answers.unity3d.com/questions/20086/bumpmap-colors.html
		 */

		for (int i = 0; i < this._mapSize; i++) {
			for (int j = 0; j < this._mapSize; j++) {
				float random = Random.Range (0.0f, 10.0f);
				if (random <= 1.0f) {
					Texture.SetPixel(i, j, Color.blue);
					NormalMap.SetPixel(i, j, Color.black);
				} else if (random > 1.0f && random <= 8.0f) {
					Texture.SetPixel(i, j, Color.green);
					NormalMap.SetPixel (i, j, Color.grey);
				} else {
					Texture.SetPixel(i, j, Color.gray);
					NormalMap.SetPixel(i, j, Color.white);
				}
			}
		}



		// Apply all SetPixel calls
		Texture.Apply ();
		NormalMap.Apply();


		// connect texture to material of GameObject this script is attached to
		this.renderer.material.mainTexture = Texture;
		this.renderer.material.SetTexture("_BumpMap", NormalMap);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	private int[,] _generateMap ()
	{
		int[,] mapTiles = new int[this._mapSize, this._mapSize];

		//Initialize everything as grass
		for (int i = 0; i < this._mapSize; i++) {
			for (int j = 0; j < this._mapSize; j++) {
				mapTiles [i, j] = _GRASS;
			}
		}

		//Now do some randomization
		int numRivers = Random.Range (0, 4);

		for (int i = 0; i < numRivers; i++) {

		}

		return mapTiles;
	}


}
