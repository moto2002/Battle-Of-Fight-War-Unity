using UnityEngine;
using System.Collections;

public class Rifleman : Unit 
{

	// Use this for initialization
	public override void Start () 
	{
		this.spriteBottomLeftX = 0.0f;
		this.spriteBottomLeftY = 32.0f;

		base.Start ();

		this.speed = 0.25f;
	}


	void OnMouseEnter()
	{
		//Color MouseOverColor = new Color (128.0f/255.0f, 255.0f / 255.0f, 128.0f / 255.0f);
		this._UnitSprite.SetColor (Color.green);
	}

}
