using UnityEngine;
using System.Collections;

public class BattleInfo
{
	
	
	public int numSoldiers = 0;
	public int numSoldiersLost = 0;
	
	public int numEnemies = 0;
	public int numEnemiesKilled = 0;
	
	
	// Use this for initialization
	public BattleInfo() 
	{
		this.numSoldiers = GameObject.FindGameObjectsWithTag("GoodGuy").Length;	
	}
	
	
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	
}
