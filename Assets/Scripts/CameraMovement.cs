using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	private float _camSpeed = 10.0f;
	private float _zoomSpeed = 200.0f;

	public bool forcedMove = false;
	public bool cameraFocusedOnEvent = false;

	//This is the target we want to focus on, not where the camera should be
	//So always subtract z from this value to get where want the camera to be
	public Vector3 TargetPosition;

	// Use this for initialization
	void Start () 
	{
	
	}


	// Update is called once per frame
	void Update () 
	{
		Vector3 trueForward = new Vector3 (0.0f, 0.0f, +1.0f);

		float translationX = 0.0f;
		float translationZ = 0.0f;

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

			//We reached the target goal, now notify user what happen
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

			this.transform.position += this.transform.forward * this._zoomSpeed * mouseScrollTranslation;
			this.transform.position += this.transform.right * this._camSpeed * translationX;
			this.transform.position += trueForward * this._camSpeed * translationZ;
		}


		/**
		if (Input.GetKeyDown(KeyCode.Q)) {
			this.transform.position += this.transform.forward * mouseScrollTranslation;
		} else if (Input.GetKeyDown(KeyCode.E)) {
			this.transform.position += this.transform.forward * -mouseScrollTranslation;
		}
		*/

		//this.transform.Translate(translationX, this.transform.position.y, translationZ);
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
}
