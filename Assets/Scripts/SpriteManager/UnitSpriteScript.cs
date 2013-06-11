using UnityEngine;
using System.Collections;

public class UnitSpriteScript : MonoBehaviour {


	private Sprite _UnitSprite;

	protected float spriteSheetWidth = 64.0f;
	protected float spriteSheetHeight = 64.0f;

	protected float bodyWidth = 64.0f;
	protected float bodyHeight = 64.0f;

	protected float bodyBottomLeftX = 64.0f;
	protected float bodyBottomLeftY = 0.0f;

	// Use this for initialization
	void Start () 
	{
		GameObject UnitSpriteManager = GameObject.Find("UnitSpriteManager");
		SpriteManager SpriteManagerScript = UnitSpriteManager.GetComponent<SpriteManager> ();

		Vector2 SpriteStart = new Vector2 ((bodyBottomLeftX / spriteSheetWidth), 1.0f - (bodyBottomLeftY / spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((bodyWidth / spriteSheetWidth), (bodyHeight / spriteSheetHeight));

		this._UnitSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
		//SpriteManagerScript.AddSprite(this.gameObject, 1, 1, 0, 48, 48, 48, false);
		this._UnitSprite.SetDrawLayer(-(int)this.gameObject.transform.position.z);

	}
	
	// Update is called once per frame
	void Update () 
	{
		this._UnitSprite.Transform();
		this._UnitSprite.SetDrawLayer(-(int)this.gameObject.transform.position.z);
	}


}
