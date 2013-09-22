using UnityEngine;
using System.Collections;

public class EnemyBase : Base 
{

	private int _timeOfLastSpawn = 0;
	private int _timeBetweenSpawns = 0;

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();

		GameObject LevelInfoObj = GameObject.Find ("LevelInfo");
		if (LevelInfoObj == null) {
			return;
		}

		LevelInfo Info = LevelInfoObj.GetComponent<LevelInfo>();
		int minSpawns = Info.minSpawnsFromEnemySpawner;
		int maxSpawns = Info.maxSpawnsFromEnemySpawner;
		this._timeBetweenSpawns = Info.timeBetweenSpawns;

		this._numSpawns = Random.Range (minSpawns, maxSpawns);

		this.friendlyTag = "Monster";
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		if (this.Units.Count < this._numSpawns && (int)Time.time > this._timeOfLastSpawn + this._timeBetweenSpawns) {

			this._timeOfLastSpawn = (int)Time.time;
			Debug.Log ("SPAWN TIME " + this._timeOfLastSpawn);
			Vector3 SpawnPosition = new Vector3 (this.gameObject.transform.position.x, 0.45f, this.gameObject.transform.position.z);
			GameObject NewUnitObj = Instantiate(this.UnitPrefab, SpawnPosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)) as GameObject;
			this.Units.Add(NewUnitObj);

			GameObject PlayerBaseObj =  GameObject.Find("PlayerBase");
			Unit NewUnit = NewUnitObj.GetComponent<Unit>();
			NewUnit.setGoalPosition (PlayerBaseObj.gameObject.transform.position);
		}
	}
}
