using UnityEngine;
using System.Collections;

public class BadGuy : Unit 
{

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
	}

	void OnMouseEnter()
	{
		this._UnitSprite.SetColor (Color.red);
	}

}
