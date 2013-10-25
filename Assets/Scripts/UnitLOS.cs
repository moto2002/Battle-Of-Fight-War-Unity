using UnityEngine;
using System.Collections;

public class UnitLOS : MonoBehaviour {

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
				
		OtherUnit.visibleToEnemy = true;
		
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
				
		OtherUnit.visibleToEnemy = false;
		//Debug.Log("LOST ENEMY");
	}
	
	
	
	
	
			
}