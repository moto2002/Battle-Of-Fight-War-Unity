using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamUnitHandler : MonoBehaviour {

	public GameObject UnitPrefab;

	public ArrayList Units;

	public int team = 0;

	public static int TEAM_GOOD_GUYS = 1;
	public static int TEAM_BAD_GUYS = 2;

	protected int _numSpawns = 0;

	// Use this for initialization
	public virtual void Start () 
	{
		this.Units = new ArrayList ();

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
		}
	}


	// Update is called once per frame
	public virtual void Update () 
	{
	
	}
}
