using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {

	public GameObject UnitPrefab;

	public ArrayList Units;

	public string friendlyTag = "";

	protected int _numSpawns = 0;

	// Use this for initialization
	public virtual void Start () 
	{
		this.Units = new ArrayList ();

		/**
		//Find all current units and make them noncollidable with this object (spawner/base)
		GameObject[] GoodGuys = GameObject.FindGameObjectsWithTag("GoodGuy");
		GameObject[] Monsters = GameObject.FindGameObjectsWithTag("Monster");

		List<GameObject> AllUnitsList = new List<GameObject> ();
		AllUnitsList.AddRange (GoodGuys);
		AllUnitsList.AddRange (Monsters);

		GameObject[] AllUnits = AllUnitsList.ToArray ();
		foreach (GameObject Unit in AllUnits) {
			//Can't ignore collisions with yourself or the method freaks out
			Physics.IgnoreCollision(Unit.collider, this.collider);
			//Debug.Log ("IGNORING COLLISION");
		} */

		this.friendlyTag = "GoodGuy";
	}


	// Update is called once per frame
	public virtual void Update () 
	{
	
	}


	public void OnTriggerEnter (Collider OtherObject)
	{
		Debug.Log ("Unit in base");
		if (OtherObject.gameObject.tag != this.friendlyTag) {
			return;
		}

		Unit UnitInBase = OtherObject.gameObject.GetComponent<Unit> ();

		if (UnitInBase != null) { //We actually have a unit
			UnitInBase.inBase = true;
		}
	}


	public void OnTriggerExit (Collider OtherObject)
	{
		if (OtherObject.gameObject.tag != this.friendlyTag) {
			return;
		}

		Unit UnitInBase = OtherObject.gameObject.GetComponent<Unit> ();

		if (UnitInBase != null) { //We actually have a unit
			UnitInBase.inBase = false;
		}
	}
}
