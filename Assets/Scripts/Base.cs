using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : NeutralBase {

	public GameObject UnitPrefab;

	public ArrayList Units  = new ArrayList ();

	public string friendlyTag = "";
	public string enemyTag = "";

	protected int _numSpawns = 0;
	
	protected bool _captured = false;


	// Use this for initialization
	public virtual void Start () 
	{
		this.friendlyTag = "GoodGuy";
		this.enemyTag = "Monster";
	}


	public override void OnTriggerEnter (Collider OtherObject)
	{
		if (OtherObject.tag == this.friendlyTag) {
			this._UnitEnteredBase(OtherObject);
		}
	}
	
	
	public override void OnTriggerExit (Collider OtherObject)
	{
		if (OtherObject.tag == this.friendlyTag) {
			this._UnitExitedBase(OtherObject);
		}
	}


	/**
	 * Need to do this for enemies of the base because we don't want a base to count as captured until
	 * enemies reach its center
	 */
	public void OnTriggerStay(Collider OtherObject)
	{
		//Check capture for enemies
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
					StartCoroutine(this._baseCaptured());	
				}
			}
			
			return;
		}
	}


	public void OnDestroy()
	{
		foreach (Unit UnitInBase in this._UnitsInBase) {
			UnitInBase.inBase = false;
		}
	}


	protected virtual IEnumerator _baseCaptured()
	{
		if (this._captured) {
			return false;	
		}
		
		this._captured = true;
		Instantiate (Resources.Load("Prefabs/Fire"), this.transform.position, Quaternion.identity);

		//Quaternion TempQuat = Quaternion.identity;
		//TempQuat.x = 0
		//Fire.transform.rotation = TempQuat;
		
		yield return new WaitForSeconds(3);
		
		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		if (LevelInfoObj != null) {
			LevelInfo LevelInfo = LevelInfoObj.GetComponent<LevelInfo>();
			LevelInfo.setPlayerLost();
			LevelInfo.setStatusUpdateLocation (this.transform.position);
		}
		
		yield return new WaitForSeconds(1);
	}


}
