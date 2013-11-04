using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{


	public GameObject SelectedUnit = null;
	
	public ArrayList PotentialSelectedUnits = new ArrayList();
	
	private Sprite _SelectSprite = null;
	private Sprite _HealthSprite = null;
	
	private Sprite _UnitDestinationSprite = null;
	private GameObject _DestFlagObj = null;
	
	private string _ownershipTag = "GoodGuy"; 


	// Use this for initialization
	void Start () 
	{	
		SpriteManager SpriteManagerScript = GameObject.Find ("MainSpriteManager").GetComponent<SpriteManager>();
		Vector2 SpriteDimensions = new Vector2 ((SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetWidth), (SpriteInfo.spriteStandardSize / SpriteInfo.spriteSheetHeight));
		
		this._DestFlagObj = Instantiate(Resources.Load("Prefabs/DestinationFlag"), this.transform.position, Quaternion.identity) as GameObject;
		//Dimensions for unit select box
		Vector2 FlagSpriteStart = new Vector2 ((SpriteInfo.destinationFlagBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.destinationFlagBottomLeftY / SpriteInfo.spriteSheetHeight));
		
		this._UnitDestinationSprite = SpriteManagerScript.AddSprite(this._DestFlagObj, 1.0f, 1.0f, FlagSpriteStart, SpriteDimensions, false);
		this._UnitDestinationSprite.hidden = true;
		this._UnitDestinationSprite.drawLayer = 998;
			
		//Dimensions for unit select box
		Vector2 SelectSpriteStart = new Vector2 ((SpriteInfo.selectBoxBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.selectBoxBottomLeftY / SpriteInfo.spriteSheetHeight));
		Vector2 HealthSpriteStart = new Vector2 ((SpriteInfo.healthBarBottomLeftX / SpriteInfo.spriteSheetWidth), 1.0f - (SpriteInfo.healthBarBottomLeftY / SpriteInfo.spriteSheetHeight));

		//pick a very large number for these UI sprites so they're drawn last; for some reason MoveToFront sucks
		this._SelectSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SelectSpriteStart, SpriteDimensions, false);
		this._SelectSprite.drawLayer = 999;
		this._SelectSprite.hidden = true;

		this._HealthSprite = SpriteManagerScript.AddSprite(this.gameObject, 1.0f, 1.0f, HealthSpriteStart, SpriteDimensions, false);
		this._HealthSprite.offset.y = 0.60f;
		this._HealthSprite.drawLayer = 1000;
		this._HealthSprite.hidden = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.SelectedUnit == null) {
			return;	
		}
		
		Unit TargetUnit = this.SelectedUnit.GetComponent<Unit>();
		if (TargetUnit.visibleToEnemy == 0 && this._ownershipTag != this.SelectedUnit.tag) {
			this.hideSelectSprite();
			this.SelectedUnit = null;
			return;
		}
		
		//this._SelectSprite.clientTransform = this.SelectedUnit.transform;
		//this._HealthSprite.clientTransform = this.SelectedUnit.transform;
		
		this._SelectSprite.Transform ();

		//Determine color of health sprite based on current health
		this._HealthSprite.Transform ();
		
		if (TargetUnit.currentAction == Unit.CURRENT_ACTION_MOVING && this._ownershipTag == this.SelectedUnit.tag) {
			
			this._DestFlagObj.transform.position = new Vector3(TargetUnit.GoalPosition.x + 0.35f, 0.60f, TargetUnit.GoalPosition.z);
			this._UnitDestinationSprite.Transform();
			this._UnitDestinationSprite.hidden = false;

		} else { //do not display movement goal

			this._UnitDestinationSprite.hidden = true;
			
		}
		
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
		this._SelectSprite.client = this.SelectedUnit;
		this._HealthSprite.client = this.SelectedUnit;
		
		this._SelectSprite.hidden = false;
		this._HealthSprite.hidden = false;
	}
	
	
	public void hideSelectSprite()
	{
		this._SelectSprite.hidden = true;
		this._HealthSprite.hidden = true;
		this._UnitDestinationSprite.hidden = true;
	}
	
	
	public void removeSelectedUnitAndSprite()
	{
		if (this.SelectedUnit != null) {
			Unit AlreadySelectedUnit = this.SelectedUnit.GetComponent<Unit>();		
			AlreadySelectedUnit.selected = false;
		}
		
		this.SelectedUnit = null;
		this.hideSelectSprite();
	}



}
