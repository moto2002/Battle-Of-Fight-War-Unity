using UnityEngine;
using System.Collections;

public class TrooperSpriteScript : MonoBehaviour {


	private Sprite _TrooperSprite;

	protected float spriteSheetWidth = 384.0f;
	protected float spriteSheetHeight = 384.0f;

	protected float bodyWidth = 64.0f;
	protected float bodyHeight = 32.0f;

	protected float bodyBottomLeftX = 0.0f;
	protected float bodyBottomLeftY = 32.0f;

	// Use this for initialization
	void Start () 
	{
		GameObject TrooperSpriteManager = GameObject.Find("TrooperSpriteManager");
		SpriteManager SpriteManagerScript = TrooperSpriteManager.GetComponent<SpriteManager> ();

		Vector2 SpriteStart = new Vector2 ((bodyBottomLeftX / spriteSheetWidth), 1.0f - (bodyBottomLeftY / spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((bodyWidth / spriteSheetWidth), (bodyHeight / spriteSheetHeight));

		this._TrooperSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
		//SpriteManagerScript.AddSprite(this.gameObject, 1, 1, 0, 48, 48, 48, false);
		this._TrooperSprite.SetDrawLayer(-(int)this.gameObject.transform.position.z);

	}
	
	// Update is called once per frame
	void Update () 
	{
		this._TrooperSprite.Transform();
		this._TrooperSprite.SetDrawLayer(-(int)this.gameObject.transform.position.z);
	}


}
