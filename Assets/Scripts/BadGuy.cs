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
	
	
	public override void Update()
	{
		//Sprite graphics stuff
		
		if (this.visibleToEnemy <= 0) {
			if (this._UnitSprite.color.a > 0.0f) {
				Color Invisibo = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				this._UnitSprite.SetColor(Invisibo);
				if (this.selected) {
					this.hideSelectSprite();
				}
			}
		} else {
			if (this._UnitSprite.color.a <= 0.0f) {
				Color Visibo = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				this._UnitSprite.SetColor(Visibo);
			}
		}
		
		base.Update();	
	}

}
