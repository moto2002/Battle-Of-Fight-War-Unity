using UnityEngine;
using System.Collections;

public class BadGuy : Unit 
{

	// Use this for initialization
	public override void Start () 
	{
		this.TeamColor = new Color(184.0f / 255.0f, 116.0f / 255.0f, 116.0f / 255.0f);
		base.Start ();
	}

	
	void OnMouseEnter()
	{
		this._setAllSpriteColors(Color.red);
	}
	
	
	public override void Update()
	{
		//Sprite graphics stuff
		
		if (this.visibleToEnemy <= 0) {
				
			this._setSoldierSpriteVisible(false);
			if (this.selected) {
				Player PlayerScript = GameObject.Find ("Player").GetComponent<Player>();
				PlayerScript.hideSelectSprite();
			}

			this.removeStatusSprite();
		} else {
			this._setSoldierSpriteVisible(true);
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
