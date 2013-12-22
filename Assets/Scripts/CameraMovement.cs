using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	private float _camSpeed = 10.0f;
	private float _zoomSpeed = 200.0f;

	public bool forcedMove = false;
	public bool cameraFocusedOnEvent = false;


	public const float X_MIN = -10.0f;
	public const float X_MAX = +10.0f;
	public const float Y_MIN = 6.0f;
	public const float Y_MAX = 8.5f;
	public const float Z_MIN = -20.0f;
	public const float Z_MAX = -5.0f;


	//This is the target we want to focus on, not where the camera should be
	//So always subtract z from this value to get where want the camera to be
	public Vector3 TargetPosition;

	// Use this for initialization
	void Start () 
	{
		Vector3 PlayerBasePos = GameObject.Find("PlayerBase").transform.position;
		//this.transform.position = new Vector3(PlayerBasePos.x, PlayerBasePos.y + 7.0f, PlayerBasePos.z - 5.0f);

		this.transform.position = new Vector3(0.0f, Y_MAX, Z_MIN);
	}


	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 trueForward = new Vector3 (0.0f, 0.0f, +1.0f);

		float translationX = 0.0f;
		float translationZ = 0.0f;

		Vector3 OldTransformPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

		if (this.forcedMove) {

			float targetZ = this.TargetPosition.z - (this.transform.position.y / 0.45f);
			//Move at "input" speeds of -1 or +1
			translationX = (this.TargetPosition.x - this.transform.position.x) / Mathf.Abs((this.TargetPosition.x - this.transform.position.x));
			translationZ = (targetZ - this.transform.position.z) / Mathf.Abs((targetZ - this.transform.position.z));

			float distanceToX = Mathf.Abs (this.TargetPosition.x - this.transform.position.x);
			if (distanceToX <= 0.10f) {
				translationX = 0;
			} else {
				translationX *= 0.60f;
			}

			float distanceToZ = Mathf.Abs (targetZ - this.transform.position.z);
			if (distanceToZ <= 0.10f) {
				translationZ = 0;
			} else {
				translationZ *= 0.60f;
			}

			//translationX = (this.TargetPosition.x - this.transform.position.x) * 0.1f;
			//translationZ = ((this.TargetPosition.z -  (this.transform.position.y / 0.45f)) - this.transform.position.z) * 0.1f;

			//Debug.Log (translationX);
			//Debug.Log (translationZ);

			translationX *= 0.02f;
			translationZ *= 0.02f;

			this.transform.position += this.transform.right * this._camSpeed * translationX;
			this.transform.position += trueForward * this._camSpeed * translationZ;

			//Debug.Log (Vector3.Distance (this.transform.position, this.TargetPosition));
			float yzRatio = Mathf.Abs(this.transform.position.y / (this.TargetPosition.z - this.transform.position.z ));

			//float d = Vector3.Distance (this.transform.position, this.TargetPosition);
			//float idealD = Vector3.Distance (new Vector3(this.TargetPosition.x, this.transform.position.x, targetZ), this.TargetPosition);

			//Debug.Log ("yzRatio: " + yzRatio + ", targetZ: " + targetZ);

			//Make sure camera is in front of our target position (more negative z) and far away enough from it
			if (this.transform.position.z <= this.TargetPosition.z && 0.44f <= yzRatio && yzRatio <= 0.46f && distanceToX <= 0.10f) {
				this.forcedMove = false;
				this.cameraFocusedOnEvent = true;
			}

		} else {

			//These values are all between -1 and 1
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

		/**
		if (Input.GetKeyDown(KeyCode.Q)) {
			this.transform.position += this.transform.forward * mouseScrollTranslation;
		} else if (Input.GetKeyDown(KeyCode.E)) {
			this.transform.position += this.transform.forward * -mouseScrollTranslation;
		}
		*/

		//Debug.Log(this.transform.position);
		this._keepCameraWithinBounds(OldTransformPosition);
	}
	
	
	public bool gameEventRequiresCameraMovement(int gameEvent)
	{
		switch (gameEvent) {
			
			case LevelInfo.GAME_EVENT_PLAYER_WON:
			case LevelInfo.GAME_EVENT_PLAYER_LOST:
			case LevelInfo.GAME_EVENT_OBJECTIVE_SECURED:
				return true;
			default:
				return false;
		}
	}


	public bool isForcedMove()
	{
		return this.forcedMove;
	}


	public void setForcedMove(bool val)
	{
		this.forcedMove = val;
	}


	public bool isFocusedOnEvent()
	{
		return this.cameraFocusedOnEvent;
	}


	public void setFocusedOnEvent(bool val)
	{
		this.cameraFocusedOnEvent = val;
	}


	public void setFocusPosition(Vector3 FocusPosition)
	{
		this.TargetPosition = FocusPosition;
	}


	private void _keepCameraWithinBounds(Vector3 OldTransformPosition)
	{
		//Restricting camera position
		if (Mathf.Abs(this.transform.position.x) >= X_MAX) {
			this.transform.position = new Vector3(OldTransformPosition.x, this.transform.position.y, this.transform.position.z);
		}
		
		if (this.transform.position.z <= Z_MIN || this.transform.position.z >= Z_MAX) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, OldTransformPosition.z);
		}
		
		if (this.transform.position.y <= Y_MIN || this.transform.position.y >= Y_MAX) {
			this.transform.position = new Vector3(this.transform.position.x, OldTransformPosition.y, this.transform.position.z);
		}
	}
}
