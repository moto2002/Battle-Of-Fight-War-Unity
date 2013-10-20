using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : NeutralBase {

	public GameObject UnitPrefab;

	public ArrayList Units;

	public string friendlyTag = "";
	public string enemyTag = "";

	protected int _numSpawns = 0;


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
	
	
	public void OnTriggerStay(Collider OtherObject)
	{
		if (OtherObject.gameObject.tag == this.enemyTag) {
			
			Vector3 spherePosition = new Vector3(
				this.transform.position.x,
				this.transform.position.y + 0.5f,
				this.transform.position.z
			);
			Collider[] ObjsInCenterOfBase = Physics.OverlapSphere(spherePosition, 0.005f);
			foreach (Collider ObjectWithin in ObjsInCenterOfBase) {
				//Debug.Log(ObjectWithin.name);
				if (ObjectWithin.tag == this.enemyTag) {
					this._baseCaptured ();	
				}
			}
			
		}	
	}


	protected virtual void _baseCaptured()
	{
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		if (LevelInfoObj == null) {
			return;
		}

		LevelInfo LevelInfo = LevelInfoObj.GetComponent<LevelInfo>();
		LevelInfo.setPlayerLost();
		LevelInfo.setStatusUpdateLocation (this.transform.position);
	}

}
