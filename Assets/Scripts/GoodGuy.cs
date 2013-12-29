using UnityEngine;
using System.Collections;

public class GoodGuy : Unit 
{

	// Use this for initialization
	public override void Start () 
	{
		this.TeamColor = new Color(97.0f / 255.0f, 136.0f / 255.0f, 189.0f / 255.0f);
		base.Start ();
	}


	void OnMouseEnter()
	{
		//Color MouseOverColor = new Color (128.0f/255.0f, 255.0f / 255.0f, 128.0f / 255.0f);
		this._setAllSpriteColors(Color.green);
	}


	protected override void _removeFromHomeBaseList()
	{

	}

}
