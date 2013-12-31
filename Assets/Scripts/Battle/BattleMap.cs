using UnityEngine;
using System.Collections;
using Pathfinding;

//Yaaaay!


public class BattleMap : MonoBehaviour {

	private int _textureSize = 512;

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

	//GameObjects for Player, Textures, etc
	public GameObject PlayerObject;

	private uint _PENALTY_GRASS = 100;
	private uint _PENALTY_FOREST = 200;
	private uint _PENALTY_MOUNTAIN = 500;
	private uint _PENALTY_ROAD = 0;

	//Number of tiles we want x by z
	public int sizeZ = 10;
	public int sizeX = 10;
	private float _tileSize = 1.0f; //In Unity Coords

	// Use this for initialization
	void Start () 
	{
		this.buildMesh();
		this._detailTexture();
		//-------------------------------------------------------------------------------------------------------------
		//Redoing texture to look nicer
			
	}
	
	// Update is called once per frame
	void Update () 
	{

	}


	void OnDestroy()
	{
		//this.renderer.material.mainTexture = this._OriginalTexture;
	}


	private void _detailTexture()
	{
		this._OriginalTexture = (Texture2D)this.renderer.material.mainTexture;
		Texture2D NewTexture = new Texture2D(this._textureSize, this._textureSize, TextureFormat.RGB24, true);
		
		Color[] NewMainTexPixels = new Color[this._textureSize * this._textureSize];
		for (int i = 0; i < this._textureSize; i++) {
			for (int j = 0; j < this._textureSize; j++) {
				Color PixelColor = this._OriginalTexture.GetPixel(i, j);
				
				int	smallTextureX = i % 32;
				int	smallTextureY = j % 32;
				
				int pixelPosition = this._textureSize * j + i;
				
				//Get the AI path node closest to this pixel
				//THIS IS SO AWESOME IT'S FUCKING SWEET
				float vectorX = (i / 25.6f) - 10.0f; 
				float vectorZ = (j / 25.6f) - 10.0f; 
				Vector3 ClosestVector = new Vector3(vectorX ,0.0f, vectorZ);
				
				//Node PathNode = AstarPath.active.GetNearest (ClosestVector).node;
				
				//Debug.Log (PixelColor.r + "," + PixelColor.g + "," + PixelColor.b);
				if (PixelColor == Color.green) {
					NewMainTexPixels[pixelPosition] = this.GrassTexture.GetPixel (smallTextureX, smallTextureY);
					
					//PathNode.penalty = this._PENALTY_GRASS;
					//PathNode.tags = GRASS;
				} else if (PixelColor == Color.blue) {
					NewMainTexPixels[pixelPosition] = this.WaterTexture.GetPixel (smallTextureX, smallTextureY);
					
					//PathNode.walkable = false;
					//PathNode.tags = WATER;
				} else if (PixelColor.r == (128.0f/255.0f) && PixelColor.g == (64.0f/255.0f)) { //Mountain brown
					NewMainTexPixels[pixelPosition] = this.MountainTexture.GetPixel (smallTextureX, smallTextureY); 
					
					//PathNode.penalty = this._PENALTY_MOUNTAIN;
					//PathNode.tags = MOUNTAIN;
				} else if (PixelColor.g == (128.0f/255.0f)) { //Forest green
					NewMainTexPixels[pixelPosition] = this.ForestTexture.GetPixel (smallTextureX, smallTextureY);
					
					//PathNode.penalty = this._PENALTY_FOREST;
					//PathNode.tags = FOREST;
				} else if (PixelColor.r == (127.0f/255.0f) && PixelColor.g == (127.0f/255.0f) && PixelColor.b == (127.0f/255.0f)) {
					NewMainTexPixels[pixelPosition] = this.RoadTexture.GetPixel (smallTextureX, smallTextureY);
					
					//PathNode.penalty = this._PENALTY_ROAD;
					//PathNode.tags = ROAD;
				}
				
				//Updating the actual node with its changes in the active grid graph?
				//GraphUpdateObject GUO = new GraphUpdateObject();
				//GUO.Apply (PathNode);
			}
		}
		
		NewTexture.SetPixels (NewMainTexPixels);
		NewTexture.Apply();
		this.renderer.material.mainTexture = NewTexture;
	}


	public void buildMesh()
	{
		int numTiles = this.sizeX * this.sizeZ;
		int numTriangles = numTiles * 2;

		//For example, on a side note that there are 4 vertices for 3 triangles
		int numXVertices = this.sizeX + 1;
		int numZVertices = this.sizeZ + 1;
		int numVertices = numXVertices * numZVertices;


		Vector3[] Vertices = new Vector3[numVertices];
		Vector3[] Normals = new Vector3[numVertices];
		Vector2[] UVCoords = new Vector2[numVertices];
		
		int[] triangleVertices = new int[numTriangles * 3];

		//First we generate the vertices
		int x = 0;
		int z = 0;
		for (z = 0; z < numZVertices; z++) {
			for (x = 0; x < numXVertices; x++) {

				Vertices[z * numXVertices + x] = new Vector3(x * this._tileSize, 0.0f, z * this._tileSize);
				Normals[z * numXVertices + x] = Vector3.up;
				UVCoords[z * numXVertices + x] = new Vector2( (float)x / numXVertices, (float)z / numZVertices);
			}
		}
		Debug.Log ("Map vertices built");

		//Then we assign vertices to triangles1
		//Note the order in which we assign these vertices.. can go clockwise or CCW
		for (z = 0; z < this.sizeZ; z++) {
			for (x = 0; x < this.sizeX; x++) {
				int squareIndex = z * this.sizeX + x;
				int triangleOffset = squareIndex * 6;
				triangleVertices[triangleOffset + 0] = z * numXVertices + x 				+ 0;
				triangleVertices[triangleOffset + 1] = z * numXVertices + x + numXVertices 	+ 0;
				triangleVertices[triangleOffset + 2] = z * numXVertices + x + numXVertices 	+ 1;

				triangleVertices[triangleOffset + 3] = z * numXVertices + x 				+ 0;
				triangleVertices[triangleOffset + 4] = z * numXVertices + x + numXVertices 	+ 1;
				triangleVertices[triangleOffset + 5] = z * numXVertices + x + 			  	+ 1;
			}
		}
		Debug.Log ("Map triangles assigned");

		Mesh NewMesh = new Mesh();
		NewMesh.vertices = Vertices;
		NewMesh.triangles = triangleVertices;
		NewMesh.normals = Normals;
		NewMesh.uv = UVCoords;

		MeshFilter ObjMeshFilter =  this.GetComponent<MeshFilter>();
		ObjMeshFilter.mesh = NewMesh;
		Debug.Log("Done building mesh");

		MeshCollider MapMeshCollider = this.GetComponent<MeshCollider>();
		MapMeshCollider.sharedMesh = NewMesh;
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

}
