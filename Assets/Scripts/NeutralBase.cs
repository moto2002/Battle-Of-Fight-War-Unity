using UnityEngine;
using System.Collections;

public class NeutralBase : MonoBehaviour 
{

	public ArrayList _UnitsInBase = new ArrayList();

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
	
	}


	public virtual void FixedUpdate () 
	{
		foreach (Unit UnitInBase in this._UnitsInBase) {
			UnitInBase.inBase = true;
		}

	}


	public virtual void OnTriggerEnter (Collider OtherObject)
	{
		this._UnitEnteredBase(OtherObject);
	}


	public virtual void OnTriggerExit (Collider OtherObject)
	{
		this._UnitExitedBase(OtherObject);
	}


	protected void _UnitEnteredBase(Collider OtherObject)
	{
		Unit UnitInBase = OtherObject.gameObject.GetComponent<Unit> ();
		if (UnitInBase != null) { //We actually have a unit
			UnitInBase.inBase = true;
			this._UnitsInBase.Add(UnitInBase);
		}
	}


	protected void _UnitExitedBase(Collider OtherObject)
	{
		Unit UnitInBase = OtherObject.gameObject.GetComponent<Unit> ();
		if (UnitInBase != null) { //We actually have a unit
			UnitInBase.inBase = false;
			this._UnitsInBase.Remove(UnitInBase);
		}
	}
}
