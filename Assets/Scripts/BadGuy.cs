using UnityEngine;
using System.Collections;

public class BadGuy : Unit 
{

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
		
		this.speed = 0.40f;
	}

	void OnMouseEnter()
	{
		this._UnitSprite.SetColor (Color.red);
	}

}
