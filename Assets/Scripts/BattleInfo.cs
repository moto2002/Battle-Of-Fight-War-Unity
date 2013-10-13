using UnityEngine;
using System.Collections;

public class BattleInfo
{
	
	
	private int _numSoldiers = 0;
	private int _numSoldiersLost = 0;
	
	private int _numEnemies = 0;
	private int _numEnemiesKilled = 0;
	
	
	// Use this for initialization
	public BattleInfo() 
	{
		this._numSoldiers = GameObject.FindGameObjectsWithTag("GoodGuy").Length;	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void updateNumSoldiersLost(int addNumSoldiersLost)
	{
		this._numSoldiersLost += addNumSoldiersLost;
	}
	
	
	void updateNumEnemiesKilled(int addNumEnemiesKilled)
	{
		this._numEnemiesKilled += addNumEnemiesKilled;	
	}
	
	
}
