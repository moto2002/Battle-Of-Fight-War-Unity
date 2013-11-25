using UnityEngine;
using System.Collections;

public class EnemyBase : Base 
{

	public int minSpawns = 1;
	public int maxSpawns = 3;
	public int timeBetweenSpawns = 30;

	private int _timeOfLastSpawn = 0;



	// Use this for initialization
	public override void Start () 
	{
		base.Start ();

		this._numSpawns = Random.Range (minSpawns, maxSpawns);

		this.friendlyTag = "Monster";
		this.enemyTag = "GoodGuy";
	}
	
	// Update is called once per frame
	public void FixedUpdate () 
	{
		if ((int)Time.fixedTime > (this._timeOfLastSpawn + this.timeBetweenSpawns) && this.Units.Count < this._numSpawns) {

			this._timeOfLastSpawn = (int)Time.fixedTime;
			Debug.Log ("SPAWN TIME " + this._timeOfLastSpawn);
			Vector3 SpawnPosition = new Vector3 (this.gameObject.transform.position.x, 0.45f, this.gameObject.transform.position.z);

			GameObject NewUnitObj = Instantiate(this.SpawnedUnit, SpawnPosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)) as GameObject;
			Unit NewUnit = NewUnitObj.GetComponent<Unit>();

			this.Units.Add(NewUnit);
			NewUnit.setHomeBase(this.gameObject);

			GameObject PlayerBaseObj =  GameObject.Find("PlayerBase");
			NewUnit.setGoalPosition (PlayerBaseObj.gameObject.transform.position);

		}
	}


	public virtual void removeUnitFromList(Unit UnitToRemove)
	{
		this.Units.Remove(UnitToRemove);
		this._timeOfLastSpawn = (int)Time.fixedTime; //Reset the "last spawn" time
	}


	protected override IEnumerator _baseCaptured()
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
		if (LevelInfoObj == null) {
			Debug.Log("ENEMY BASE TRIED TO USE LEVEL INFO BUT NOT FOUND. ERRORRRRR!!!");
			return false;
		}
		
		LevelInfo LevelInfo = LevelInfoObj.GetComponent<LevelInfo>();
		LevelInfo.objectiveCaptured();
		LevelInfo.setStatusUpdateLocation (this.transform.position);

		Destroy (this);
		Destroy (this.gameObject);
	}


}
