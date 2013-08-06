using UnityEngine;
using System.Collections;

public class Monster1 : Unit 
{

	// Use this for initialization
	public override void Start () 
	{
		this.spriteBottomLeftX = 0.0f;
		this.spriteBottomLeftY = 256.0f;

		base.Start ();
		
		this.speed = 0.40f;
	}

	void OnMouseEnter()
	{
		this._UnitSprite.SetColor (Color.red);
	}

}
