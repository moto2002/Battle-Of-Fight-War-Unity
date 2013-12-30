using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour {

	public const float X_MIN = -10.0f;
	public const float X_MAX = +10.0f;
	public const float Y_MIN = 6.0f;
	public const float Y_MAX = 8.5f;
	public const float Z_MIN = -20.0f;
	public const float Z_MAX = -5.0f;

	private float _camSpeed = 10.0f;
	private float _zoomSpeed = 200.0f;


	// Use this for initialization
	void Start () 
	{
		this.transform.position = new Vector3(0.0f, Y_MAX, Z_MIN);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 trueForward = new Vector3 (0.0f, 0.0f, +1.0f);

		float translationX = 0.0f;
		float translationZ = 0.0f;

		translationX = Input.GetAxis ("Horizontal");
		translationZ = Input.GetAxis ("Vertical");
		float mouseScrollTranslation = Input.GetAxis ("Mouse ScrollWheel");
		
		//Debug.Log (translationX);
		//Debug.Log (translationZ);
		
		translationX *= Time.deltaTime;
		translationZ *= Time.deltaTime;
		mouseScrollTranslation *= Time.deltaTime;
		
		//translationZ = Mathf.Cos (rotationZInRad) * translationZ;
		//Debug.Log(rotationXInRad);
		
		this.transform.position += (this.transform.right * this._camSpeed * translationX);
		this.transform.position += (trueForward * this._camSpeed * translationZ);
		
		Vector3 ZoomPosition = (this.transform.forward * this._zoomSpeed * mouseScrollTranslation) + this.transform.position;
		this.transform.position = new Vector3(this.transform.position.x, ZoomPosition.y, this.transform.position.z);
	}
}
