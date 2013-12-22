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
		this._SpriteRenderer.color = Color.red;
	}
	
	
	public override void Update()
	{
		//Sprite graphics stuff
		
		if (this.visibleToEnemy <= 0) {
				
			this._SpriteRenderer.enabled = false;
			if (this.selected) {
				Player PlayerScript = GameObject.Find ("Player").GetComponent<Player>();
				PlayerScript.hideSelectSprite();
			}

			this.removeStatusSprite();
		} else {
			this._SpriteRenderer.enabled = true;
		}
		
		base.Update();	
	}


	protected override void _removeFromHomeBaseList()
	{
		GameObject HomeBaseObj = this.HomeBase;
		if (HomeBaseObj == null) {
			return;
		}
		EnemyBase Base = HomeBaseObj.GetComponent<EnemyBase>();

		Base.removeUnitFromList(this);
	}

}
