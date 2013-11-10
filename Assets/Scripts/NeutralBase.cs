using UnityEngine;
using System.Collections;

public class NeutralBase : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
	
	}


	public virtual void OnTriggerStay (Collider OtherObject)
	{
		Unit UnitInBase = OtherObject.gameObject.GetComponent<Unit> ();
		if (UnitInBase != null) { //We actually have a unit
			UnitInBase.inBase = true;
		}
	}


	public virtual void OnTriggerExit (Collider OtherObject)
	{
		Unit UnitInBase = OtherObject.gameObject.GetComponent<Unit> ();
		if (UnitInBase != null) { //We actually have a unit
			UnitInBase.inBase = false;
		}
	}
}
