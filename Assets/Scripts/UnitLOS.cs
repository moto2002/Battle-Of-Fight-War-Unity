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
		//GameObject OtherParentObject = OtherObject.transform.parent;
		Debug.Log("LOS found something");
		/**
		if (this.transform.parent.gameObject.tag == OtherParentObject.tag) {
			return;	
		}
		
		Debug.Log("FOUND ENEMY");
		*/
	}
	
	
	
	
	
			
}