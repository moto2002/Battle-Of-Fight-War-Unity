using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : NeutralBase {

	public GameObject UnitPrefab;

	public ArrayList Units;

	public string friendlyTag = "";
	public string enemyTag = "";

	protected int _numSpawns = 0;
	protected int _numFriendlyUnits = 0;


	// Use this for initialization
	public virtual void Start () 
	{
		this.Units = new ArrayList ();

		this.friendlyTag = "GoodGuy";
		this.enemyTag = "Monster";
	}


	// Update is called once per frame
	public override void Update () 
	{
	
	}


	public override void OnTriggerEnter (Collider OtherObject)
	{
		//Debug.Log ("Unit in base");
		if (OtherObject.gameObject.tag != this.friendlyTag) {
			return;
		}

		this._numFriendlyUnits++;
		base.OnTriggerEnter (OtherObject);
	}


	public override void OnTriggerExit (Collider OtherObject)
	{
		if (OtherObject.gameObject.tag != this.friendlyTag) {
			return;
		}

		this._numFriendlyUnits--;
		base.OnTriggerExit (OtherObject);
	}


	public virtual void OnTriggerStay(Collider OtherObject) 
	{
		if (OtherObject.gameObject.tag != this.enemyTag) {
			return; //Nothing to do here for non-enemy units
		}

		if (this._numFriendlyUnits <= 0) {
			this._setGameOver ();
		}
	}


	protected virtual void _setGameOver()
	{
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		if (LevelInfoObj == null) {
			return;
		}

		LevelInfo LevelInfo = LevelInfoObj.GetComponent<LevelInfo>();
		LevelInfo.setPlayerLost();
	}

}
