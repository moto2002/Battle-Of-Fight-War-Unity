using UnityEngine;
using System.Collections;
using Pathfinding;


public class Unit : MonoBehaviour 
{


	protected Sprite _UnitSprite;
	protected Sprite _SelectSprite;
	protected Sprite _HealthSprite;
	
	protected GameObject _PlayerObject;
	protected GameObject _MainSpriteManager;

	//Public variables
	public string unitClass;
	public bool selected = false;
	public bool inCombat = false;
	public bool inBase = false;
	public string currentAction;

	//Setup variables
	public bool generateNames = false;
	public GameObject CombatEffects = null;
	
	public int numMembers;

	//Default unit stats for prefab (Should set these through Unity)
	public float speed = 0.25f;
	public float health = 100.0f;

	public ArrayList CombatTargets;

	public ArrayList SquadMembers;


	//The combat effects relevant to this unit; needs to be removed when combat is over
	protected GameObject _CombatEffectsInstance = null;

	protected int _timeOfLastAttack = 0;
	protected int _timeOfLastHeal = 0;

	/**
	 * Pathfinding variables
	 */

	protected CharacterController _Controller;

	//The calculated path
	public Path PathToFollow;

	//Whether or not the unit should currently be assigned a path
	public bool shouldSeekPath = false;

	//The waypoint we are currently moving towards
	protected int currentWaypoint = 0;

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 0.1f;
	
	public Vector3 GoalPosition = new Vector3(0.0f, -1.0f, 0.0f); //We should never get a neg't y value

	//Set these through the game engine
	public float spriteBottomLeftX;
	public float spriteBottomLeftY;


	// STATIC SHIT APPLICABLE TO ALL UNITS ----------------------------------------

	public static string CURRENT_ACTION_HOLDING = "Holding Position";
	public static string CURRENT_ACTION_MOVING = "Moving";
	public static string CURRENT_ACTION_COMBAT = "In Combat";

	//-----------------------------------------------------------------------------



	// Use this for initialization
	public virtual void Start () 
	{
		this.CombatTargets = new ArrayList ();
		this.SquadMembers = new ArrayList ();

		this._PlayerObject = GameObject.Find("Player");
		this._MainSpriteManager = GameObject.Find("MainSpriteManager");

		this._Controller = this.GetComponent<CharacterController> ();

		SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

		Vector2 SpriteStart = new Vector2 ((this.spriteBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (this.spriteBottomLeftY / SpriteInfo.spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((SpriteInfo.spriteWidth / SpriteInfo.spriteSheetWidth), (SpriteInfo.spriteHeight / SpriteInfo.spriteSheetHeight));

		this._UnitSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
		//SpriteManagerScript.AddSprite(this.gameObject, 1, 1, 0, 48, 48, 48, false);
		//this._UnitSprite.SetDrawLayer(-(int)this.gameObject.transform.position.z);

		/**
		 * COLLISION STUFF NO LONGER NEEDED since every map asset is now just a non-collidable trigger
		//Make sure this unit is allowed to go through other friendly units
		GameObject[] Objects = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
		for (int i = 0; i < Objects.Length; i++) {
			if (this.gameObject == Objects [i]) {
				continue;
			}
			//Can't ignore collisions with yourself or the method freaks out
			Physics.IgnoreCollision(Objects[i].collider, this.collider);
		}

		//Make sure this unit is allowed to go through bases/spawn points (enemy or otherwise)
		Objects = GameObject.FindGameObjectsWithTag("PlayerBase");
		for (int i = 0; i < Objects.Length; i++) {
			if (this.gameObject == Objects [i]) {
				continue;
			}
			//Can't ignore collisions with yourself or the method freaks out
			Physics.IgnoreCollision(Objects[i].collider, this.collider);
		}
		Objects = GameObject.FindGameObjectsWithTag("EnemySpawn");
		for (int i = 0; i < Objects.Length; i++) {
			if (this.gameObject == Objects [i]) {
				continue;
			}
			//Can't ignore collisions with yourself or the method freaks out
			Physics.IgnoreCollision(Objects[i].collider, this.collider);
		} 
		*/
		
		GameObject LevelInfoObject = GameObject.Find ("LevelInfo");
		LevelInfo Info = LevelInfoObject.GetComponent<LevelInfo> ();
		string[] possibleNames = Info.getPossibleNames();

		string unitName = "";
		SquadMemberCreator SquaddieCreator = new SquadMemberCreator();
		for (int i = 0; i < this.numMembers; i++) {
			if (this.generateNames) {
				unitName = possibleNames [Random.Range (0, possibleNames.Length - 1)];
			} else {
				unitName = this.unitClass + " " + (i + 1);
			}
			SquadMember NewMember = SquaddieCreator.createSquadMember(this.unitClass, unitName);
			this.SquadMembers.Add (NewMember);
		}

		this.currentAction = CURRENT_ACTION_HOLDING;
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
				this.currentAction = CURRENT_ACTION_HOLDING;
			} else {
				//Debug.Log ("looking for new path");
				Seeker AISeeker = this.GetComponent<Seeker> ();
				AISeeker.StartPath (this.transform.position, this.GoalPosition, pathSeekComplete);
				this.currentAction = CURRENT_ACTION_MOVING;
			}

			//Need to stop seeking path immediately. Apparently the pathfinding algorithm doesn't do everything in one frame
			//(which is pretty awesome), so we shouldn't keep restarting it
			this.shouldSeekPath = false; 
		}

		this._UnitSprite.Transform();
		//Multiply the z by 100 so we get a more accurate drawLayer reading
		this._UnitSprite.drawLayer = (int)(this.gameObject.transform.position.z * -100);

		if (this.selected) {
			this._SelectSprite.Transform ();

			//Determine color of health sprite based on current health
			this._HealthSprite.Transform ();

			float healthWidth = this.health / 100.0f;
			//For some reason calling SetSizeXY() fixed the offset problem (i.e. offset wasn't doing jack shit)
			this._HealthSprite.SetSizeXY (healthWidth, 0.3f);

			if (this.health > 70.0f) {
				this._HealthSprite.SetColor (Color.green);
			} else if (this.health > 30.0f) {
				this._HealthSprite.SetColor (Color.yellow);
			} else {
				this._HealthSprite.SetColor (Color.red);
			}
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

		GameObject GUIObject = GameObject.Find ("GameGUI");
		GameGui GUI = GUIObject.GetComponent<GameGui> ();

		if (this.selected) {
			if (this._SelectSprite == null) {
				SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

				if (PlayerScript.SelectedUnit != null) {
					//Unselect previously-selected unit
					Unit UnitScript = PlayerScript.SelectedUnit.GetComponent<Unit> ();
					UnitScript.removeSelectionBox ();
				}

				PlayerScript.SelectedUnit = this.gameObject;

				//Dimensions for unit select box
				Vector2 SelectSpriteStart = new Vector2 ((SpriteInfo.selectBoxBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.selectBoxBottomLeftY / SpriteInfo.spriteSheetHeight));
				Vector2 HealthSpriteStart = new Vector2 ((SpriteInfo.healthBarBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.healthBarBottomLeftY / SpriteInfo.spriteSheetHeight));
				Vector2 SpriteDimensions = new Vector2 ((SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetWidth), (SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetHeight));

				//pick a very large number for these UI sprites so they're drawn last; for some reason MoveToFront sucks
				this._SelectSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SelectSpriteStart, SpriteDimensions, false);
				this._SelectSprite.drawLayer = 999;

				this._HealthSprite = SpriteManagerScript.AddSprite(this.gameObject, 1.0f, 1.0f, HealthSpriteStart, SpriteDimensions, false);
				this._HealthSprite.offset.y = 0.60f;
				this._HealthSprite.drawLayer = 1000;
				//SpriteManagerScript.MoveToFront (this._SelectSprite);
			}

			GUI.SelectedUnit = this.gameObject;

		} else {
			SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();
			PlayerScript.SelectedUnit = null;
			if (this._SelectSprite != null) { //Deselecting unit

				SpriteManagerScript.RemoveSprite (this._SelectSprite);
				this._SelectSprite = null;
			}

			if (this._HealthSprite != null) {

				SpriteManagerScript.RemoveSprite (this._HealthSprite);
				//Debug.Log ("Removed health sprite");

				this._HealthSprite = null;

			}

			GUI.SelectedUnit = null;
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
		//Debug.Log ("Path complete! Error? " + CompletedPath.error);
		if (!CompletedPath.error) {
			this.PathToFollow = CompletedPath;
			//Reset the waypoint counter
			this.currentWaypoint = 0;
			this.shouldSeekPath = false;
		}
	}


	public void FixedUpdate () 
	{
		if (this.health <= 0.0f) {
			this.die ();
			return;
		}

		//Start the healing process if we're in a base
		if (this.inBase && this.currentAction == CURRENT_ACTION_HOLDING && this.health < 100.0f) {
			this.heal ();
			return;
		}

		if (this.PathToFollow == null || this.inCombat) { //do not move if in combat
			if (this.inCombat) {
				//Do combat stuff once cooldown is done
				if ((int)Time.time >= this._timeOfLastAttack + 2) {
					foreach (GameObject EnemyUnitObject in this.CombatTargets) {

						if (EnemyUnitObject == null) {
							continue;
						}
						Unit EnemyUnit = EnemyUnitObject.GetComponent<Unit> ();
						this.attack (EnemyUnit);
						this._timeOfLastAttack = (int)Time.time;

					}
				}
			}

			return;
		}
		
		//Did we reach our goal?
		if (this.currentWaypoint >= this.PathToFollow.vectorPath.Count) {
			//Debug.Log (Vector3.Distance (this.transform.position, this.PathToFollow.vectorPath[this.currentWaypoint-1]));
			//Vector3 Goal = this.PathToFollow.vectorPath [this.currentWaypoint-1];
			//Debug.Log ("Reached point " + Goal.x + "," + Goal.y + "," + Goal.z);
			this.PathToFollow = null;
			//this.shouldSeekPath = false;
			//Debug.Log ("End Of Path Reached");
			this.currentAction = CURRENT_ACTION_HOLDING;
			return;
		}

		//Direction to the next waypoint
		Vector3 Direction = (this.PathToFollow.vectorPath[currentWaypoint]-transform.position).normalized;

		//Determine if we should modify speed based on the node terrain type
		float speedModifier = 1.0f;
		switch (AstarPath.active.GetNearest (this.transform.position).node.tags) {

			case Map.FOREST:
				speedModifier = 0.5f;
					break;
			case Map.MOUNTAIN:
				speedModifier = 0.3f;
				break;
			default:
				break;
		}

		Direction *= (this.speed * speedModifier) * Time.fixedDeltaTime;

		this.gameObject.transform.position = new Vector3 (
			this.gameObject.transform.position.x + Direction.x,
			this.gameObject.transform.position.y,
			this.gameObject.transform.position.z + Direction.z
		);
		//this._Controller.SimpleMove (Direction);

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		/**
		 * I suspect there may be a problem here with the "distance until waypoint considered reached" variable
		 * If it's too low then the object will never get to said waypoint due to the direction calculation fucking up
		 * in-between waypoints
		 * TODO: add a special condition for the last waypoint (i.e. the goal) because we need the unit to reach
		 * the exact goal coordinates
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

		this.currentAction = CURRENT_ACTION_MOVING;
	}


	public void removeSelectionBox()
	{
		SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

		SpriteManagerScript.RemoveSprite (this._SelectSprite);
		SpriteManagerScript.RemoveSprite (this._HealthSprite);
		this._SelectSprite = null;
		this._HealthSprite = null;

		this.selected = false;
	}


	public void OnTriggerEnter (Collider OtherObject)
	{
		//Debug.Log ("Units collided");
		//Debug.Log (this.gameObject.tag);
		//Debug.Log (OtherObject.gameObject.tag);

		if (OtherObject.gameObject.tag != this.gameObject.tag) {

			//Debug.Log ("object tags are different");
			Unit OtherUnit = OtherObject.gameObject.GetComponent<Unit> ();

			if (OtherUnit != null) { //Two unfriendly units have collided oh noes!
				//Debug.Log ("Combat!");
				if (!this.inCombat && this.CombatEffects != null) { //Only create combat effects unless already in combat
					//Vector3 EffectsPosition = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z); 
					GameObject CombatEffects = Instantiate (this.CombatEffects, this.transform.position, Quaternion.identity) as GameObject;
					this._CombatEffectsInstance = CombatEffects;
				}
				this.inCombat = true;
				this.CombatTargets.Add (OtherObject.gameObject);
				this.currentAction = CURRENT_ACTION_COMBAT;
				//Debug.Log ("Added combat target");
			}

		}
	}


	public void OnTriggerExit(Collider OtherObject)
	{
		//Debug.Log ("ON TRIGGER EXITED");
	}


	public void attack(Unit EnemyUnit)
	{
		foreach (SquadMember Squaddie in this.SquadMembers) {
			EnemyUnit.damage (Squaddie.attackPower);
		}
	}




	public void damage(float damage)
	{
		//Do damage to a random squad member
		int randomGuyToDamageIndex = Random.Range (0, this.SquadMembers.Count);
		SquadMember DamagedGuy = SquadMembers [randomGuyToDamageIndex] as SquadMember;
		DamagedGuy.health -= damage;
		if (DamagedGuy.health <= 0.0f) { //Oh noes squaddie is dead
			DamagedGuy.die();
			SquadMembers.RemoveAt(randomGuyToDamageIndex);
		}

		if (this.SquadMembers.Count <= 0) { //Everyone's dead, screw it
			this.health = 0.0f;
			return;
		}

		float totalHealth = 0.0f;
		float maxHealth = 0.001f; //So we don't divide by 0
		foreach (SquadMember Squaddie in this.SquadMembers) {
			totalHealth += Squaddie.health;
			maxHealth += 100.0f;
		}

		//Since health is a percentage
		this.health = (totalHealth/maxHealth) * 100.0f;
	}


	public void die()
	{
		string enemyTag = "";
		if (this.gameObject.tag == "GoodGuy") {
			enemyTag = "Monster";
		} else {
			enemyTag = "GoodGuy";
		}

		//Remove this guy from all enemy target lists, if any
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag(enemyTag);

		int deadGuyId = this.gameObject.GetInstanceID ();
		foreach (GameObject EnemyUnitObject in Enemies) {
			Unit EnemyUnit = EnemyUnitObject.GetComponent<Unit> ();
			bool foundDeadGuy = false;
			foreach (GameObject TargetUnit in EnemyUnit.CombatTargets) {
				//Debug.Log ("LOOKING THROUGH ENEMY TARGETS FOR DEAD GUY " + deadGuyId + ", comparing with GUY " + TargetUnit.GetInstanceID());
				if (TargetUnit.GetInstanceID () == deadGuyId) {
					//Debug.Log ("REMOVING DEAD GUY " + deadGuyId);
					EnemyUnit.CombatTargets.Remove (TargetUnit);
					foundDeadGuy = true;
					//Debug.Log ("Target " + deadGuyId + " removed");
				}

				//if this dead guy is the enemy's last target, then they are no longer in combat
				if (EnemyUnit.CombatTargets.Count == 0) {
					EnemyUnit.removeFromCombat();
				}

				if (foundDeadGuy) {
					break;
				}
			}
		}

		Destroy (this);
		Destroy (this.gameObject);
	}


	public void heal()
	{
		if ((int)Time.time <= this._timeOfLastHeal + 2) {
			return;
		}

		this._timeOfLastHeal = (int)Time.time;

		float totalHealth = 0.0f;
		float maxHealth = 0.001f; //So we don't divide by 0
		foreach (SquadMember Squaddie in this.SquadMembers) {
			Squaddie.health += 2.0f;
			totalHealth += Squaddie.health;
			maxHealth += 100.0f;
		}

		//Since health is a percentage
		this.health = (totalHealth/maxHealth) * 100.0f;
	}


	void OnDestroy ()
	{
		if (this.gameObject == null) {
			return;
		}

		if (this._MainSpriteManager != null) {
			SpriteManager SpriteManager = this._MainSpriteManager.GetComponent<SpriteManager> ();

			SpriteManager.RemoveSprite (this._UnitSprite);
			if (this._SelectSprite != null) {
				SpriteManager.RemoveSprite (this._SelectSprite);
			}
			if (this._HealthSprite != null) {
				SpriteManager.RemoveSprite (this._HealthSprite);
			}
		}

		if (this._CombatEffectsInstance != null) {
			GameObject.Destroy (this._CombatEffectsInstance);
		}
	}


	public void removeFromCombat()
	{
		this.inCombat = false;
		if (this._CombatEffectsInstance != null) {
			GameObject.Destroy (this._CombatEffectsInstance);
		}
	}



}
