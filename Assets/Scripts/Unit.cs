using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {


	private Sprite _UnitSprite;
	private Sprite _SelectSprite;

	protected static float spriteSheetWidth = 192.0f;
	protected static float spriteSheetHeight = 192.0f;

	protected static float selectBoxBottomLeftX = 0.0f;
	protected static float selectBoxBottomLeftY = 192.0f;

	protected static float spriteStandardSize = 64.0f;

	protected float spriteWidth = 64.0f;
	protected float spriteHeight = 64.0f;

	protected float spriteBottomLeftX = 0.0f;
	protected float spriteBottomLeftY = 64.0f;

	public bool selected = false;

	// Use this for initialization
	void Start () 
	{
		GameObject MainSpriteManager = GameObject.Find("MainSpriteManager");
		SpriteManager SpriteManagerScript = MainSpriteManager.GetComponent<SpriteManager> ();

		Vector2 SpriteStart = new Vector2 ((spriteBottomLeftX / spriteSheetWidth), 1.0f - (spriteBottomLeftY / spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((spriteWidth / spriteSheetWidth), (spriteHeight / spriteSheetHeight));

		this._UnitSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
		//SpriteManagerScript.AddSprite(this.gameObject, 1, 1, 0, 48, 48, 48, false);
		this._UnitSprite.SetDrawLayer(-(int)this.gameObject.transform.position.z);

	}
	
	// Update is called once per frame
	void Update () 
	{
		this._UnitSprite.Transform();
		//this._UnitSprite.SetDrawLayer((int)this.gameObject.transform.position.z);

		if (this._SelectSprite != null) {
			this._SelectSprite.Transform();
			//this._SelectSprite.SetDrawLayer((int)this.gameObject.transform.position.z + 5);
		}
	}


	void OnMouseExit()
	{
		this._UnitSprite.SetColor (Color.white);
	}


	void OnMouseDown()
	{
		this.selected = !this.selected;
		GameObject MainSpriteManager = GameObject.Find("MainSpriteManager");
		SpriteManager SpriteManagerScript = MainSpriteManager.GetComponent<SpriteManager> ();

		if (this.selected) {
			if (this._SelectSprite == null) {
				//Dimensions for unit select box
				Vector2 SpriteStart = new Vector2 ((selectBoxBottomLeftX / spriteSheetWidth), 1.0f - (selectBoxBottomLeftY / spriteSheetHeight));
				Vector2 SpriteDimensions = new Vector2 ((spriteStandardSize / spriteSheetWidth), (spriteStandardSize / spriteSheetHeight));

				this._SelectSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
				SpriteManagerScript.MoveToFront (this._SelectSprite);
			}
		} else {
			if (this._SelectSprite != null) {
				SpriteManagerScript.RemoveSprite (this._SelectSprite);
				this._SelectSprite = null;
			}
		}
	}


	void OnMouseEnter()
	{
		Color MouseOverColor = new Color (128.0f/255.0f, 255.0f / 255.0f, 128.0f / 255.0f);
		this._UnitSprite.SetColor (Color.green);

		//Below is commented-out bullshit code that didn't work but might be 
		//My original goal was to customize the individual pixels within the sprite
		/**
		GameObject UnitSpriteManager = GameObject.Find("UnitSpriteManager");

		Texture2D OriginalSprite = (Texture2D)UnitSpriteManager.renderer.material.mainTexture;
		Texture2D NewTexture = new Texture2D(64, 64, TextureFormat.ARGB32, true);

		Color[] NewMainTexPixels = new Color[64 * 64];
		Color DarkYellow = new Color (200.0f/255.0f, 175.0f / 255.0f, 35.0f / 255.0f);
		for (int i = 0; i < 64; i++) {
			for (int j = 0; j < 64; j++) {
				Color PixelColor = OriginalSprite.GetPixel(i, j);

				int pixelPosition = 64 * j + i;
				if (PixelColor.b > (200.0f / 255.0f) && PixelColor.a > 0.0f) {
					NewMainTexPixels [pixelPosition] = Color.yellow;
				} else if (PixelColor.b > (100.0f / 255.0f)) { //Darker color
					NewMainTexPixels [pixelPosition] = DarkYellow;
				} else if (PixelColor.a == 0.0f) {
					NewMainTexPixels [pixelPosition] = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				} else {
					NewMainTexPixels [pixelPosition] = PixelColor;
				}
			}
		}

		NewTexture.SetPixels (NewMainTexPixels);
		NewTexture.Apply();

		UnitSpriteManager.renderer.material.mainTexture = NewTexture;
		SpriteManager SpriteManagerScript = UnitSpriteManager.GetComponent<SpriteManager> ();

		Vector2 SpriteStart = new Vector2 ((bodyBottomLeftX / spriteSheetWidth), 1.0f - (bodyBottomLeftY / spriteSheetHeight));
		Vector2 SpriteDimensions = new Vector2 ((bodyWidth / spriteSheetWidth), (bodyHeight / spriteSheetHeight));

		this._UnitSprite = SpriteManagerScript.AddSprite(this.gameObject, 1, 1, SpriteStart, SpriteDimensions, false);
		*/
	}


}
