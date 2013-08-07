using UnityEngine;
using System.Collections;

public class GoodGuy : Unit 
{

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
	}


	void OnMouseEnter()
	{
		//Color MouseOverColor = new Color (128.0f/255.0f, 255.0f / 255.0f, 128.0f / 255.0f);
		this._UnitSprite.SetColor (Color.green);
	}

}
