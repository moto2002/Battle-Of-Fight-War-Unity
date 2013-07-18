using UnityEngine;
using System.Collections;
using Pathfinding;


public class Unit : MonoBehaviour {


	protected Sprite _UnitSprite;
	protected Sprite _SelectSprite;

	protected static float spriteSheetWidth = 256.0f;
	protected static float spriteSheetHeight = 256.0f;

	protected static float selectBoxBottomLeftX = 0.0f;
	protected static float selectBoxBottomLeftY = 192.0f;

	protected static float spriteStandardSize = 32.0f;

	protected float spriteWidth = 32.0f;
	protected float spriteHeight = 32.0f;

	protected float spriteBottomLeftX = 0.0f;
	protected float spriteBottomLeftY = 64.0f;
	
	protected GameObject _PlayerObject;
	protected GameObject _MainSpriteManager;
	//Public variables

	public bool selected = false;

	/**
	 * Pathfinding variables
	 */

	protected CharacterController _Controller;

	//The calculated path
	public Path PathToFollow;

	//The AI's speed per second
	public float speed = 100;

	//Whether or not the unit should currently be assigned a path
	public bool shouldSeekPath = false;

	//The waypoint we are currently moving towards
	protected int currentWaypoint = 0;

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 0.1f;
	
	public Vector3 GoalPosition = new Vector3(0.0f, -1.0f, 0.0f); //We should never get a neg't y value

	// Use this for initialization
	public virtual void Start () 
	{
		this._PlayerObject = GameObject.Find("Player");
		this._MainSpriteManager = GameObject.Find("MainSpriteManager");

		this._Controller = this.GetComponent<CharacterController> ();

		SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

		Vector2 SpriteStart = new Vector2 ((spriteBottomLeftX / spriteSheetWidth), 1.0f - (spriteBottomLeftY / spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((spriteWidth / spriteSheetWidth), (spriteHeight / spriteSheetHeight));

		this._UnitSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
		//SpriteManagerScript.AddSprite(this.gameObject, 1, 1, 0, 48, 48, 48, false);
		this._UnitSprite.SetDrawLayer(-(int)this.gameObject.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (this.shouldSeekPath && this.PathToFollow == null) {

			if (
				Mathf.Abs (this.GoalPosition.x - this.transform.position.x) <= 0.5f && 
				Mathf.Abs (this.GoalPosition.z - this.transform.position.z) <= 0.5f
			    ) {
				//Goal is too close; don't look for a path, so do nothing
			} else {
				Debug.Log ("looking for new path");
				Seeker AISeeker = this.GetComponent<Seeker> ();
				AISeeker.StartPath (this.transform.position, this.GoalPosition, pathSeekComplete);
			}

			//Need to stop seeking path immediately. Apparently the pathfinding algorithm doesn't do everything in one frame
			//(which is pretty awesome), so we shouldn't keep restarting it
			this.shouldSeekPath = false; 
		}

		this._UnitSprite.Transform();
		//this._UnitSprite.SetDrawLayer((int)this.gameObject.transform.position.z);

		if (this._SelectSprite != null) {
			this._SelectSprite.Transform();
			//this._SelectSprite.SetDrawLayer((int)this.gameObject.transform.position.z + 5);
		}
	}


	void OnMouseExit()
	{
		this._UnitSprite.SetColor (Color.white);
	}


	void OnMouseDown()
	{
		this.selected = !this.selected;

		Player PlayerScript = this._PlayerObject.GetComponent<Player> ();

		if (this.selected) {
			if (this._SelectSprite == null) {
				SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

				if (PlayerScript.SelectedUnit != null) {
					Unit UnitScript = PlayerScript.SelectedUnit.GetComponent<Unit> ();
					UnitScript.removeSelectionBox ();
				}

				PlayerScript.SelectedUnit = this.gameObject;

				//Dimensions for unit select box
				Vector2 SpriteStart = new Vector2 ((selectBoxBottomLeftX / spriteSheetWidth), 1.0f - (selectBoxBottomLeftY / spriteSheetHeight));
				Vector2 SpriteDimensions = new Vector2 ((spriteStandardSize / spriteSheetWidth), (spriteStandardSize / spriteSheetHeight));

				this._SelectSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
				SpriteManagerScript.MoveToFront (this._SelectSprite);
			}
		} else {
			if (this._SelectSprite != null) { //Deselecting a unit
				SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

				PlayerScript.SelectedUnit = null;

				SpriteManagerScript.RemoveSprite (this._SelectSprite);
				this._SelectSprite = null;
			}
		}

	}


	/**
	void OnMouseEnter()
	{
		//Below is commented-out bullshit code that didn't work but might be 
		//My original goal was to customize the individual pixels within the sprite
		/**
		GameObject UnitSpriteManager = GameObject.Find("UnitSpriteManager");

		Texture2D OriginalSprite = (Texture2D)UnitSpriteManager.renderer.material.mainTexture;
		Texture2D NewTexture = new Texture2D(64, 64, TextureFormat.ARGB32, true);

		Color[] NewMainTexPixels = new Color[64 * 64];
		Color DarkYellow = new Color (200.0f/255.0f, 175.0f / 255.0f, 35.0f / 255.0f);
		for (int i = 0; i < 64; i++) {
			for (int j = 0; j < 64; j++) {
				Color PixelColor = OriginalSprite.GetPixel(i, j);

				int pixelPosition = 64 * j + i;
				if (PixelColor.b > (200.0f / 255.0f) && PixelColor.a > 0.0f) {
					NewMainTexPixels [pixelPosition] = Color.yellow;
				} else if (PixelColor.b > (100.0f / 255.0f)) { //Darker color
					NewMainTexPixels [pixelPosition] = DarkYellow;
				} else if (PixelColor.a == 0.0f) {
					NewMainTexPixels [pixelPosition] = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				} else {
					NewMainTexPixels [pixelPosition] = PixelColor;
				}
			}
		}

		NewTexture.SetPixels (NewMainTexPixels);
		NewTexture.Apply();

		UnitSpriteManager.renderer.material.mainTexture = NewTexture;
		SpriteManager SpriteManagerScript = UnitSpriteManager.GetComponent<SpriteManager> ();

		Vector2 SpriteStart = new Vector2 ((bodyBottomLeftX / spriteSheetWidth), 1.0f - (bodyBottomLeftY / spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((bodyWidth / spriteSheetWidth), (bodyHeight / spriteSheetHeight));

		this._UnitSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);

	} */


	public void pathSeekComplete(Path CompletedPath)
	{
		Debug.Log ("Path complete! Error? " + CompletedPath.error);
		if (!CompletedPath.error) {
			this.PathToFollow = CompletedPath;
			//Reset the waypoint counter
			this.currentWaypoint = 0;
			this.shouldSeekPath = false;
		}
	}


	public void FixedUpdate () 
	{
		if (this.PathToFollow == null) {
			//We have no path to move after yet
			return;
		}

		if (this.currentWaypoint >= this.PathToFollow.vectorPath.Count) {
			Debug.Log (Vector3.Distance (this.transform.position, this.PathToFollow.vectorPath[this.currentWaypoint-1]));
			//Vector3 Goal = this.PathToFollow.vectorPath [this.currentWaypoint-1];
			//Debug.Log ("Reached point " + Goal.x + "," + Goal.y + "," + Goal.z);
			this.PathToFollow = null;
			//this.shouldSeekPath = false;
			Debug.Log ("End Of Path Reached");
			return;
		}

		//Direction to the next waypoint
		Vector3 Direction = (this.PathToFollow.vectorPath[currentWaypoint]-transform.position).normalized;
		Direction *= this.speed * Time.fixedDeltaTime;
		this._Controller.SimpleMove (Direction);

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		/**
		 * I suspect there may be a problem here with the "distance until waypoint considered reached" variable
		 * If it's too low then the object will never get to said waypoint due to the direction calculation fucking up
		 * in-between waypoints
		 * If it's too high then the object will end up only half-assing it to their goal
		 */ 
		if (Vector3.Distance (this.transform.position, this.PathToFollow.vectorPath[this.currentWaypoint]) < 0.65f) {
			//Debug.Log (Vector3.Distance (this.transform.position, this.PathToFollow.vectorPath[this.currentWaypoint]));
			this.currentWaypoint++;
			return;
		}
	}


	public void setGoalPosition(Vector3 NewGoalPosition)
	{
		this.GoalPosition.x = NewGoalPosition.x;
		this.GoalPosition.z = NewGoalPosition.z;
		this.GoalPosition.y = 0;

		this.shouldSeekPath = true;
		this.PathToFollow = null;
		this.currentWaypoint = 0;
	}


	public void removeSelectionBox()
	{
		if (this.selected) {
			SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

			SpriteManagerScript.RemoveSprite (this._SelectSprite);
			this._SelectSprite = null;

			this.selected = false;
		}
	}


}