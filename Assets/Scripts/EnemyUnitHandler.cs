using UnityEngine;
using System.Collections;

public class EnemyUnitHandler : TeamUnitHandler 
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
			GameObject NewUnit = Instantiate(this.UnitPrefab, SpawnPosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)) as GameObject;
            this.Units.Add(NewUnit);

			GameObject MapObj =  GameObject.Find("Map");
			Map Map = MapObj.GetComponent<Map> ();
			Vector2 PlayerBasePositionV2 = Map.PlayerBasePosition;

			Vector3 PlayerBasePositionV3 = new Vector3 (PlayerBasePositionV2.x, 0.45f, PlayerBasePositionV2.y);
			Unit Unit = NewUnit.GetComponent<Unit>();
			Unit.setGoalPosition (PlayerBasePositionV3);
		}
	}
}
