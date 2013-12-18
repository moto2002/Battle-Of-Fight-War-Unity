using UnityEngine;
using System.Collections;
using Pathfinding;


public class Map : MonoBehaviour {

	private int _mapSize = 512;

	//MAKE SURE THESE #s MATCH UP WITH THE TAGS IN THE UNITY A* OBJECT
	public const int GRASS = 1;
	public const int WATER = 2;
	public const int MOUNTAIN = 3;
	public const int FOREST = 4;
	public const int ROAD = 5;

	public Texture2D GrassTexture;
	public Texture2D WaterTexture;
	public Texture2D MountainTexture;
	public Texture2D ForestTexture;
	public Texture2D RoadTexture;

	private Texture2D _OriginalTexture;

	//GameObjects
	public GameObject PlayerObject;

	private uint _PENALTY_GRASS = 100;
	private uint _PENALTY_FOREST = 200;
	private uint _PENALTY_MOUNTAIN = 500;
	private uint _PENALTY_ROAD = 0;

	// Use this for initialization
	void Start () 
	{

		//-------------------------------------------------------------------------------------------------------------
		//Redoing texture to look nicer

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

				//Get the AI path node closest to this pixel
				//THIS IS SO AWESOME IT'S FUCKING SWEET
				float vectorX = (i / 25.6f) - 10.0f; 
				float vectorZ = (j / 25.6f) - 10.0f; 
				Vector3 ClosestVector = new Vector3(vectorX ,0.0f, vectorZ);

				Node PathNode = AstarPath.active.GetNearest (ClosestVector).node;

				//Debug.Log (PixelColor.r + "," + PixelColor.g + "," + PixelColor.b);
				if (PixelColor == Color.green) {
					mapTiles [i, j] = GRASS;
					NewMainTexPixels[pixelPosition] = this.GrassTexture.GetPixel (smallTextureX, smallTextureY);

					PathNode.penalty = this._PENALTY_GRASS;
					PathNode.tags = GRASS;
				} else if (PixelColor == Color.blue) {

					mapTiles [i, j] = WATER;
					NewMainTexPixels[pixelPosition] = this.WaterTexture.GetPixel (smallTextureX, smallTextureY);

					PathNode.walkable = false;
					PathNode.tags = WATER;
				} else if (PixelColor.r == (128.0f/255.0f) && PixelColor.g == (64.0f/255.0f)) { //Mountain brown
					mapTiles [i, j] = MOUNTAIN;
					NewMainTexPixels[pixelPosition] = this.MountainTexture.GetPixel (smallTextureX, smallTextureY); 

					PathNode.penalty = this._PENALTY_MOUNTAIN;
					PathNode.tags = MOUNTAIN;
				} else if (PixelColor.g == (128.0f/255.0f)) { //Forest green
					mapTiles [i, j] = FOREST;
					NewMainTexPixels[pixelPosition] = this.ForestTexture.GetPixel (smallTextureX, smallTextureY);

					PathNode.penalty = this._PENALTY_FOREST;
					PathNode.tags = FOREST;
				} else if (PixelColor.r == (127.0f/255.0f) && PixelColor.g == (127.0f/255.0f) && PixelColor.b == (127.0f/255.0f)) {
					mapTiles [i, j] = ROAD;
					NewMainTexPixels[pixelPosition] = this.RoadTexture.GetPixel (smallTextureX, smallTextureY);

					PathNode.penalty = this._PENALTY_ROAD;
					PathNode.tags = ROAD;
				}

				//Updating the actual node with its changes in the active grid graph?
				GraphUpdateObject GUO = new GraphUpdateObject();
				GUO.Apply (PathNode);
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
				
				//Debug.Log(HitInfo.collider.tag);
				//Don't do anything if we click on the unit itself
				if (HitInfo.collider.gameObject.GetInstanceID() == PlayerScript.SelectedUnit.GetInstanceID()) {
					return;	
				}
				
				if (
					HitInfo.point.x < -10.0f || HitInfo.point.x > +10.0f || 
					HitInfo.point.z < -10.0f || HitInfo.point.z > +10.0f
					) {
					//Don't bother if it's out of map
					return;					
				}

				//Debug.Log ("Ordered Unit to go to point " + HitInfo.point);

				NNConstraint NodeConstraints = new NNConstraint();
				NodeConstraints.constrainWalkability = true;
				NodeConstraints.walkable = true;

				Node PathNode = AstarPath.active.GetNearest(HitInfo.point, NodeConstraints).node;

				/**
				Debug.Log(PathNode.tags);
				if (PathNode.tags == WATER) {
					Debug.Log("FOUND WATER");
					if (PathNode.walkable) {
						Debug.Log("FOUND WALKABLE WATER WTFFFFFF!!!");
					}
				}
				*/

				/** No longer need this check since we're constraining GetNearest to walkable nodes only
				if (!PathNode.walkable) {
					//node is not walkable, so do noffin, jon snuh
					Debug.Log ("Clicked on unwalkable node");
					return;
				}
				*/


				//Debug.DrawLine (RayFromCameraToMouseClickPoint.origin, HitInfo.point);
				Unit UnitScript = PlayerScript.SelectedUnit.GetComponent<Unit> ();

				//Debug.Log ("Hit Point: " + HitInfo.point);
				//Debug.Log ("Node Point: " + PathNode.position);

				Vector3 GoalPoint = new Vector3(
					PathNode.position.x * 0.001f,
					PathNode.position.y * 0.001f,
					PathNode.position.z * 0.001f
				);
				UnitScript.setGoalPosition (GoalPoint);
			}
		} 
	}


	void OnDestroy()
	{
		this.renderer.material.mainTexture = this._OriginalTexture;
	}


	private bool _nodeIsAlreadySetToWalkable(Node PathNode)
	{
		if (
			PathNode.tags == GRASS 		||
			PathNode.tags == ROAD 		||
			PathNode.tags == FOREST 	||
			PathNode.tags == MOUNTAIN
		) {
			return true;
		}

		return false;
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


	/**
	 * USELESS NOW
	 * Add bases/spawn points

	private void _addBases()
	{
		if (this.PlayerBasePosition.x != 0.0f && this.PlayerBasePosition.y != 0.0f) {

			Vector3 BasePosition = new Vector3 (this.PlayerBasePosition.x, 0.0f, this.PlayerBasePosition.y);
			GameObject PlayerBase = Instantiate(this.PlayerBasePrefab, BasePosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)) as GameObject;
		}

		for (int i = 0; i < this.EnemySpawnPositions.Length; i++) {
			Vector3 BasePosition = new Vector3 (this.EnemySpawnPositions[i].x, 0.0f, this.EnemySpawnPositions[i].y);
			GameObject EnemySpawn = Instantiate(this.EnemySpawnPrefab, BasePosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)) as GameObject;
		}
	}
	*/

}
