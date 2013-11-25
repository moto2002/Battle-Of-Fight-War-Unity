using UnityEngine;
using System.Collections;

public class UnitLOS : MonoBehaviour {


	private ArrayList _EnemiesInView = new ArrayList();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void OnTriggerEnter (Collider OtherObject)
	{
		GameObject OtherGameObject = OtherObject.gameObject;
		if (OtherGameObject.tag == this.transform.parent.gameObject.tag) {
			return;	
		}

		Unit OtherUnit = OtherGameObject.GetComponent<Unit>();
		if (OtherUnit == null) {
			//Not seeing a unit
			return;	
		}
				
		OtherUnit.visibleToEnemy++;
		this._EnemiesInView.Add(OtherUnit);
		
		//Debug.Log("FOUND ENEMY");
	}
	
	
	public void OnTriggerExit (Collider OtherObject)
	{
		GameObject OtherGameObject = OtherObject.gameObject;
		if (OtherGameObject.tag == this.transform.parent.gameObject.tag) {
			return;	
		}
		
		Unit OtherUnit = OtherGameObject.GetComponent<Unit>();
		if (OtherUnit == null) {
			//Not seeing a unit
			return;	
		}
				
		OtherUnit.visibleToEnemy--;
		this._EnemiesInView.Remove(OtherUnit);
		//Debug.Log("LOST ENEMY");
	}


	public void OnDestroy()
	{
		foreach (Unit VisibleEnemy in this._EnemiesInView) {
			VisibleEnemy.visibleToEnemy--;
		}
	}
	
	
			
}