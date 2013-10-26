using UnityEngine;
using System.Collections;
using Pathfinding;


public class Unit : MonoBehaviour 
{


	protected Sprite _UnitSprite;
	protected Sprite _SelectSprite;
	protected Sprite _HealthSprite;
	protected Sprite _StatusSprite;
	
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
	public int visibleToEnemy = 0;


	public ArrayList SquadMembers;

	private GameObject _Battle = null;
		
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
			NewMember.health = 50.0f;
			this.health = 50.0f;
			this.SquadMembers.Add (NewMember);
		}
		

		this.currentAction = CURRENT_ACTION_HOLDING;
		
		//LOS shit
		Vector3 LOSPosition = new Vector3(
			this.transform.position.x,
			this.transform.position.y - 0.25f,
			this.transform.position.z
		);
		GameObject NewUnitLOS = Instantiate(Resources.Load("Prefabs/UnitLOS"), LOSPosition, Quaternion.identity) as GameObject;
		NewUnitLOS.transform.parent = this.gameObject.transform;
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
		//Should probably move this to FixedUpdate		
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
		
		if (this._StatusSprite != null) {
			this._StatusSprite.Transform();	
		}

		if (this.selected) {
			this._SelectSprite.Transform ();

			//Determine color of health sprite based on current health
			this._HealthSprite.Transform ();

			float healthWidth = this.health / 100.0f;
			this._HealthSprite.SetSizeXY (healthWidth, 0.26f);

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
		ArrayList CloseUnits = this._getCloseUnits();
		if (CloseUnits.Count > 0) {
			//Debug.Log ("Selected lotsa units");
			Player PlayerScript = this._PlayerObject.GetComponent<Player>();
			
			//Make sure to add this guy to the list of potential selectables
			CloseUnits.Add(this.gameObject);
			PlayerScript.PotentialSelectedUnits = CloseUnits;
			return;
		}
		
		this.selected = !this.selected;

		if (this.selected) {
			
			this.displaySelectSprite();		

		} else {
			
			this.hideSelectSprite();
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
			
			if (this._StatusSprite == null) {
				
				SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();
				
				Vector2 SpriteStart = new Vector2 ((SpriteInfo.healingIconBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.healingIconBottomRightY / SpriteInfo.spriteSheetHeight));
				Vector2 SpriteDimensions = new Vector2 ((SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetWidth), (SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetHeight));
				
				//pick a very large number for these UI sprites so they're drawn last; for some reason MoveToFront sucks
				this._StatusSprite = SpriteManagerScript.AddSprite(this.gameObject,  0.35f, 0.35f, SpriteStart, SpriteDimensions, false);
				this._StatusSprite.drawLayer = this._UnitSprite.drawLayer + 1;
				//this._StatusSprite.drawLayer = 1001;
				this._StatusSprite.offset.x = -0.40f;
				this._StatusSprite.offset.y = +0.10f;
				//Offset doesn't take effect until we call setSizeXY
				this._StatusSprite.SetSizeXY(0.35f, 0.35f);
					
			}
			
			//Debug.Log (this._StatusSprite.offset);
			this.heal ();
			return;
		}

		if (this.PathToFollow == null || this.inCombat) { //do not move if in combat
			if (this.inCombat) {
				this.currentAction = CURRENT_ACTION_COMBAT; //Just to make sure the status is right				
			}

			return;
		}
		
		//Not healing and not in combat and not holding position
		if (this._StatusSprite != null) {
			this.removeStatusSprite();	
		}
		
		//Then we must be moving
		this.currentAction = CURRENT_ACTION_MOVING;
		
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
	
	
	public void removeStatusSprite()
	{
		SpriteManager Manager = this._MainSpriteManager.GetComponent<SpriteManager>();
		Manager.RemoveSprite(this._StatusSprite);
		this._StatusSprite = null;
	}	


	public void OnTriggerEnter (Collider OtherObject)
	{
		//Add Unit LOS logic here
		if (OtherObject.gameObject.GetComponent<Unit>() == null) {
			//Debug.Log ("Found something but no unit in it");
			return;		
		}
		
		//Debug.Log ("Units collided");
		//Debug.Log (this.gameObject.tag);
		//Debug.Log (OtherObject.gameObject.tag);
		
		if (OtherObject.gameObject.tag != this.gameObject.tag) {

			//Debug.Log ("object tags are different");
			Unit OtherUnit = OtherObject.gameObject.GetComponent<Unit> ();

			if (OtherUnit != null) { //Two unfriendly units have collided oh noes!
				//Debug.Log ("Combat!");
				
				//Neither is in battle yet				
				if (!this.inCombat && !OtherUnit.inCombat) {
					
					//Battle should appear between the two
					Vector3 BattleArea = (this.transform.position + OtherObject.transform.position) / 2.0f;
					BattleArea.y = 0.20f;
					GameObject BattleObj = Instantiate (Resources.Load("Prefabs/Battle"), BattleArea, Quaternion.identity) as GameObject;
					
					Battle NewBattle = BattleObj.GetComponent<Battle>();
					NewBattle.addUnit(this.gameObject);
					NewBattle.addUnit(OtherUnit.gameObject);
					
				} else if (this.inCombat) { //We're already in battle
					
					Battle ExistingBattle = this._Battle.GetComponent<Battle>();
					ExistingBattle.addUnit(OtherUnit.gameObject);
						
				} else { //Other guy is already in battle
					
					Battle ExistingBattle = OtherUnit.getBattle().GetComponent<Battle>();
					ExistingBattle.addUnit(this.gameObject);
				}
				
				this.inCombat = true;

				/**
				if (!this.inCombat && this.CombatEffects != null) { //Only create combat effects unless already in combat
					//Vector3 EffectsPosition = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z); 
					GameObject CombatEffects = Instantiate (this.CombatEffects, this.transform.position, Quaternion.identity) as GameObject;
					this._CombatEffectsInstance = CombatEffects;
				}
				this.CombatTargets.Add (OtherObject.gameObject);
				*/
				
				
				//Debug.Log ("Added combat target");
			}

		}
	}


	public void OnTriggerExit(Collider OtherObject)
	{
		//Debug.Log ("ON TRIGGER EXITED");
	}
	
	
	public void createCombatEffects()
	{
		if (this.CombatEffects != null) { //Only create combat effects if they're not already there!
			//Vector3 EffectsPosition = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z); 
			GameObject CombatEffects = Instantiate (this.CombatEffects, this.transform.position, Quaternion.identity) as GameObject;
			this._CombatEffectsInstance = CombatEffects;
		}
	}


	public void attack(GameObject EnemyUnitObj)
	{
		bool attackCooldownDone = (int)Time.fixedTime >= this._timeOfLastAttack + 2;
		if (!attackCooldownDone) {
			return;	
		}
		
		Unit EnemyUnit = EnemyUnitObj.GetComponent<Unit>();
		foreach (SquadMember Squaddie in this.SquadMembers) {
			if (EnemyUnit.SquadMembers.Count <= 0) {
				break;
			}
			EnemyUnit.damage (Squaddie.attackPower);
		}
		
		this._timeOfLastAttack = (int)Time.fixedTime;
	}




	public void damage(float damage)
	{
		//Do damage to a random squad member
		int randomGuyToDamageIndex = Random.Range (0, this.SquadMembers.Count);
		SquadMember DamagedGuy = SquadMembers [randomGuyToDamageIndex] as SquadMember;
		DamagedGuy.health -= damage;
		if (DamagedGuy.health <= 0.0f) { //Oh noes squaddie is dead
			SquadMembers.RemoveAt(randomGuyToDamageIndex);
			DamagedGuy.die();
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
		Destroy (this);
		Destroy (this.gameObject);
	}


	public void heal()
	{
		if ((int)Time.fixedTime <= this._timeOfLastHeal + 2) {
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
		
		if (this.health >= 100.0f) {
			if (this._StatusSprite != null) {
				this.removeStatusSprite();
			}	
		}
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
			if (this._StatusSprite != null) {
				SpriteManager.RemoveSprite(this._StatusSprite);	
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
	
	
	public void displaySelectSprite()
	{
		Player PlayerScript = this._PlayerObject.GetComponent<Player> ();

		if (this._SelectSprite == null) {
				
			SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();

			if (PlayerScript.SelectedUnit != null) {
				//Unselect previously-selected unit
				Unit UnitScript = PlayerScript.SelectedUnit.GetComponent<Unit> ();
				UnitScript.removeSelectionBox ();
			}
	
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
		
		PlayerScript.SelectedUnit = this.gameObject;
		this.selected = true;
	}
	
	
	public void hideSelectSprite()
	{
		SpriteManager SpriteManagerScript = this._MainSpriteManager.GetComponent<SpriteManager> ();
		Player PlayerScript = this._PlayerObject.GetComponent<Player> ();
		
		if (this._SelectSprite != null) { //Deselecting unit

			SpriteManagerScript.RemoveSprite (this._SelectSprite);
			this._SelectSprite = null;
		}

		if (this._HealthSprite != null) {

			SpriteManagerScript.RemoveSprite (this._HealthSprite);
			//Debug.Log ("Removed health sprite");

			this._HealthSprite = null;

		}

		PlayerScript.SelectedUnit = null;
		this.selected = false;
	}
	
	
	public GameObject getBattle()
	{
		return this._Battle;	
	}
	
	
	public void setBattle(GameObject NewBattle)
	{
		this._Battle = NewBattle;
		if (this._Battle != null) {
			this.inCombat = true;
			this.currentAction = CURRENT_ACTION_COMBAT;
		} else {
			this.inCombat = false;
			this.currentAction = CURRENT_ACTION_HOLDING;
			Destroy(this._CombatEffectsInstance);
			this._CombatEffectsInstance = null;
		}
	}
	
	
	private ArrayList _getCloseUnits()
	{
		GameObject[] GoodGuys = GameObject.FindGameObjectsWithTag("GoodGuy");
		GameObject[] BadGuys = GameObject.FindGameObjectsWithTag("Monster");
		
		GameObject[] AllUnits = new GameObject[GoodGuys.Length + BadGuys.Length];
		GoodGuys.CopyTo(AllUnits, 0);
		BadGuys.CopyTo(AllUnits, GoodGuys.Length);
		
		ArrayList CloseUnits = new ArrayList();
		foreach (GameObject OtherUnit in AllUnits) {
			
			//we don't want to compare against ourselves
			if (this.gameObject.GetInstanceID() == OtherUnit.gameObject.GetInstanceID()) {
				continue;
			}
			
			float d = Vector3.Distance(this.gameObject.transform.position, OtherUnit.gameObject.transform.position);
			if (d < 0.5f) {
				//Debug.Log(d);
				CloseUnits.Add(OtherUnit);			
			}
		}
		
		return CloseUnits;
	}



}
