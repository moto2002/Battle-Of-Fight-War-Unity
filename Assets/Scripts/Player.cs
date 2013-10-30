using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{


	public GameObject SelectedUnit = null;
	
	public ArrayList PotentialSelectedUnits = new ArrayList();
	
	private Sprite _SelectSprite = null;
	private Sprite _HealthSprite = null;
	private Sprite _StatusSprite = null;
	
	private Sprite _UnitDestination = null;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.SelectedUnit == null) {
			return;	
		}
		
		Unit TargetUnit = this.SelectedUnit.GetComponent<Unit>();
		
		//this._SelectSprite.clientTransform = this.SelectedUnit.transform;
		//this._HealthSprite.clientTransform = this.SelectedUnit.transform;
		
		this._SelectSprite.Transform ();

		//Determine color of health sprite based on current health
		this._HealthSprite.Transform ();
		
		if (TargetUnit.health > 70.0f) {
			this._HealthSprite.SetColor (Color.green);
		} else if (TargetUnit.health > 30.0f) {
			this._HealthSprite.SetColor (Color.yellow);
		} else {
			this._HealthSprite.SetColor (Color.red);
		}
		
		float healthWidth = TargetUnit.health / 100.0f;
		this._HealthSprite.SetSizeXY (healthWidth, 0.26f);
	}
	
	
	public void displaySelectSprite()
	{
		SpriteManager SpriteManagerScript = GameObject.Find ("SpriteManager").GetComponent<SpriteManager>();
		
		//Dimensions for unit select box
		Vector2 SelectSpriteStart = new Vector2 ((SpriteInfo.selectBoxBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.selectBoxBottomLeftY / SpriteInfo.spriteSheetHeight));
		Vector2 HealthSpriteStart = new Vector2 ((SpriteInfo.healthBarBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.healthBarBottomLeftY / SpriteInfo.spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetWidth), (SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetHeight));

		//pick a very large number for these UI sprites so they're drawn last; for some reason MoveToFront sucks
		this._SelectSprite = SpriteManagerScript.AddSprite(this.SelectedUnit, 1, 1, SelectSpriteStart, SpriteDimensions, false);
		this._SelectSprite.drawLayer = 999;

		this._HealthSprite = SpriteManagerScript.AddSprite(this.SelectedUnit, 1.0f, 1.0f, HealthSpriteStart, SpriteDimensions, false);
		this._HealthSprite.offset.y = 0.60f;
		this._HealthSprite.drawLayer = 1000;
		//SpriteManagerScript.MoveToFront (this._SelectSprite);	
		
	}
	
	
	public void hideSelectSprite()
	{
		SpriteManager SpriteManagerScript = GameObject.Find ("SpriteManager").GetComponent<SpriteManager>();
		
		if (this._SelectSprite != null) {

			SpriteManagerScript.RemoveSprite (this._SelectSprite);
			this._SelectSprite = null;
		}

		if (this._HealthSprite != null) {

			SpriteManagerScript.RemoveSprite (this._HealthSprite);
			//Debug.Log ("Removed health sprite");

			this._HealthSprite = null;

		}

		this.SelectedUnit = null;
	}
	
	
	public void removeSelectionBox()
	{
		SpriteManager SpriteManagerScript = GameObject.Find ("SpriteManager").GetComponent<SpriteManager> ();

		SpriteManagerScript.RemoveSprite (this._SelectSprite);
		SpriteManagerScript.RemoveSprite (this._HealthSprite);
		this._SelectSprite = null;
		this._HealthSprite = null;
	}
	
	
	public void addCombatStatusSprite()
	{
		SpriteManager SpriteManagerScript = GameObject.Find ("SpriteManager").GetComponent<SpriteManager> ();
		
		Vector2 SpriteStart = new Vector2 ((SpriteInfo.combatIconBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.combatIconBottomRightY / SpriteInfo.spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetWidth), (SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetHeight));
		
		//pick a very large number for these UI sprites so they're drawn last; for some reason MoveToFront sucks
		this._StatusSprite = SpriteManagerScript.AddSprite(this.SelectedUnit,  0.35f, 0.35f, SpriteStart, SpriteDimensions, false);
		this._StatusSprite.drawLayer = this._UnitSprite.drawLayer + 1;
		//this._StatusSprite.drawLayer = 1001;
		this._StatusSprite.offset.x = -0.40f;
		this._StatusSprite.offset.y = +0.10f;
		//Offset doesn't take effect until we call setSizeXY
		this._StatusSprite.SetSizeXY(0.35f, 0.35f);	
	}
	
	
	public void addHealingStatusSprite()
	{
		SpriteManager SpriteManagerScript = GameObject.Find ("SpriteManager").GetComponent<SpriteManager> ();
			
		Vector2 SpriteStart = new Vector2 ((SpriteInfo.healingIconBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.healingIconBottomRightY / SpriteInfo.spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetWidth), (SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetHeight));
		
		//pick a very large number for these UI sprites so they're drawn last; for some reason MoveToFront sucks
		this._StatusSprite = SpriteManagerScript.AddSprite(this.SelectedUnit, 0.35f, 0.35f, SpriteStart, SpriteDimensions, false);
		this._StatusSprite.drawLayer = this._UnitSprite.drawLayer + 1;
		//this._StatusSprite.drawLayer = 1001;
		this._StatusSprite.offset.x = -0.40f;
		this._StatusSprite.offset.y = +0.10f;
		//Offset doesn't take effect until we call setSizeXY
		this._StatusSprite.SetSizeXY(0.35f, 0.35f);
	}
	
	
	public void removeStatusSprite()
	{
		if (this._StatusSprite == null) {
			return;	
		}
		
		SpriteManager Manager = GameObject.Find ("SpriteManager").GetComponent<SpriteManager>();
		Manager.RemoveSprite(this._StatusSprite);
		this._StatusSprite = null;
	}	



}
