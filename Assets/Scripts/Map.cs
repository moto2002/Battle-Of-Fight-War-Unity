using UnityEngine;
using System.Collections;
using Pathfinding;


public class Map : MonoBehaviour {

	private int _mapSize = 512;

	public const int GRASS = 1;
	public const int WATER = 2;
	public const int MOUNTAIN = 3;
	public const int FOREST = 4;

	public Texture2D GrassTexture;
	public Texture2D WaterTexture;
	public Texture2D MountainTexture;
	public Texture2D ForestTexture;

	private Texture2D _OriginalTexture;

	public GameObject PlayerObject;


	// Use this for initialization
	void Start () 
	{
		this._OriginalTexture = (Texture2D)this.renderer.material.mainTexture;
		Texture2D NewTexture = new Texture2D(this._mapSize, this._mapSize, TextureFormat.RGB24, true);

		int[,] mapTiles = new int[this._mapSize, this._mapSize];

		Color[] NewMainTexPixels = new Color[this._mapSize * this._mapSize];
		for (int i = 0; i < this._mapSize; i++) {
			for (int j = 0; j < this._mapSize; j++) {
				Color PixelColor = this._OriginalTexture.GetPixel(i, j);

				int	smallTextureX = i % 32;
				int	smallTextureY = j % 32;

				int pixelPosition = this._mapSize * j + i;
				//Debug.Log (PixelColor.r + "," + PixelColor.g + "," + PixelColor.b);
				if (PixelColor == Color.green) {
					mapTiles [i, j] = GRASS;
					NewMainTexPixels[pixelPosition] = this.GrassTexture.GetPixel (smallTextureX, smallTextureY); 
				} else if (PixelColor == Color.blue) {
					mapTiles [i, j] = WATER;
					NewMainTexPixels[pixelPosition] = this.WaterTexture.GetPixel (smallTextureX, smallTextureY); 
				} else if (PixelColor.r == (128.0f/255.0f) && PixelColor.g == (64.0f/255.0f)) { //Mountain brown
					mapTiles [i, j] = MOUNTAIN;
					NewMainTexPixels[pixelPosition] = this.MountainTexture.GetPixel (smallTextureX, smallTextureY); 
				} else if (PixelColor.g == (128.0f/255.0f)) { //Forest green
					mapTiles [i, j] = FOREST;
					NewMainTexPixels[pixelPosition] = this.ForestTexture.GetPixel (smallTextureX, smallTextureY); 
				}

				//Get the AI path node closest to this pixel
				//Start by shooting a ray from above the middle of the map (0,0,0) to the position

				//Node PathNode = AstarPath.active.GetNearest (transform.position, NNConstraint.Default);
			}
		}

		NewTexture.SetPixels (NewMainTexPixels);
		NewTexture.Apply();
		this.renderer.material.mainTexture = NewTexture;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(1)) {
			Player PlayerScript = this.PlayerObject.GetComponent<Player> ();
			if (PlayerScript.SelectedUnit == null) {
				return;
			}

			//Gives you a ray going from camera to the designated point on the screen (x,y?)
			Ray RayFromCameraToMouseClickPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit HitInfo;

			//Last variable is the length of the ray
			//Physics.Raycast returns true if it collides with sommat; hitInfo will contain collision info in this case
			if (Physics.Raycast (RayFromCameraToMouseClickPoint, out HitInfo, 100.0f)) {
				Debug.Log ("Right-clicked on Point " + HitInfo.point);
				//Debug.DrawLine (RayFromCameraToMouseClickPoint.origin, HitInfo.point);
				Unit UnitScript = PlayerScript.SelectedUnit.GetComponent<Unit> ();
				UnitScript.setGoalPosition (HitInfo.point);
			}
		}
	}


	void OnDestroy()
	{
		this.renderer.material.mainTexture = this._OriginalTexture;
	}


	/**
	 * In case I need this for future generations
	 */
	private void _generateGrayScaleTexture()
	{
		// Create a new X x Y texture ARGB32 (32 bit with alpha) and no mipmaps
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

			}
		}

		// Apply all SetPixel calls
		NormalMap.Apply();
	}


}
