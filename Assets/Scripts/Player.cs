using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{


	public GameObject SelectedUnit = null;
	
	public ArrayList PotentialSelectedUnits = new ArrayList();
	
	protected SpriteRenderer _SelectSprite = null;
	protected SpriteRenderer _HealthSprite = null;
	protected SpriteRenderer _UnitDestinationSprite = null;

	private GameObject _DestFlagObj = null;
	
	public string ownershipTag = "GoodGuy"; 


	// Use this for initialization
	void Start () 
	{	
		this._SelectSprite = GameObject.Find("SelectSprite").GetComponent<SpriteRenderer>();
		this._HealthSprite = GameObject.Find("HealthSprite").GetComponent<SpriteRenderer>();
		this._UnitDestinationSprite = GameObject.Find("DestinationFlagSprite").GetComponent<SpriteRenderer>();

		this._SelectSprite.enabled = false;
		this._HealthSprite.enabled = false;
		this._UnitDestinationSprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.SelectedUnit == null) {
			return;	
		}
		
		Unit TargetUnit = this.SelectedUnit.GetComponent<Unit>();
		if (TargetUnit.visibleToEnemy == 0 && this.ownershipTag != this.SelectedUnit.tag) {
			this.hideSelectSprite();
			this.SelectedUnit = null;
			return;
		}
		
		//this._SelectSprite.clientTransform = this.SelectedUnit.transform;
		//this._HealthSprite.clientTransform = this.SelectedUnit.transform;

		//Determine color of health sprite based on current health

		this._SelectSprite.enabled = true;
		this._HealthSprite.enabled = true;

		this._SelectSprite.gameObject.transform.position = this.SelectedUnit.transform.position;
		this._HealthSprite.gameObject.transform.position = new Vector3(
			this.SelectedUnit.transform.position.x,
			this.SelectedUnit.transform.position.y + 0.35f,
			this.SelectedUnit.transform.position.z
		);

		if (TargetUnit.PathToFollow != null && this.ownershipTag == this.SelectedUnit.tag) {
			
			this._UnitDestinationSprite.transform.position = new Vector3(TargetUnit.GoalPosition.x + 0.22f, 0.50f, TargetUnit.GoalPosition.z);
			this._UnitDestinationSprite.enabled = true;

		} else { //do not display movement goal

			this._UnitDestinationSprite.enabled = false;
			
		}
		
		if (TargetUnit.health > 70.0f) {
			this._HealthSprite.color = Color.green;
		} else if (TargetUnit.health > 30.0f) {
			this._HealthSprite.color = Color.yellow;
		} else {
			this._HealthSprite.color = Color.red;
		}

		//Set health sprite scale
		float healthWidth = (TargetUnit.health / 100.0f) * 2.0f; //Just because I know localScale.x is 2.0f max
		//Debug.Log("Health width: " + healthWidth);
		this._HealthSprite.transform.localScale = new Vector3(healthWidth, 0.50f, 2.0f); 
	}
	
	
	public void displaySelectSprite()
	{	
		this._SelectSprite.gameObject.transform.position = this.SelectedUnit.transform.position;
		this._HealthSprite.gameObject.transform.position = this.SelectedUnit.transform.position;

		this._SelectSprite.enabled = false;
		this._HealthSprite.enabled = false;
	}
	
	
	public void hideSelectSprite()
	{
		this._SelectSprite.enabled = false;
		this._HealthSprite.enabled = false;
		this._UnitDestinationSprite.enabled = false;
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
