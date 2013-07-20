using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	private float _camSpeed = 10.0f;
	private float _zoomSpeed = 200.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float translationX = Input.GetAxis ("Horizontal");
		float translationZ = Input.GetAxis ("Vertical");
		float mouseScrollTranslation = Input.GetAxis ("Mouse ScrollWheel");

		translationX *= Time.deltaTime;
		translationZ *= Time.deltaTime;
		mouseScrollTranslation *= Time.deltaTime;

		//translationZ = Mathf.Cos (rotationZInRad) * translationZ;
		//Debug.Log(rotationXInRad);
		Vector3 trueForward = new Vector3 (0.0f, 0.0f, +1.0f);
	
		this.transform.position += this.transform.right * this._camSpeed * translationX;
		this.transform.position += trueForward * this._camSpeed * translationZ;
		this.transform.position += this.transform.forward * this._zoomSpeed * mouseScrollTranslation;

		/*
		if (Input.GetKeyDown(KeyCode.Q)) {
			this.transform.position += this.transform.forward * mouseScrollTranslation;
		} else if (Input.GetKeyDown(KeyCode.E)) {
			this.transform.position += this.transform.forward * -mouseScrollTranslation;
		}
		*/

		//this.transform.Translate(translationX, this.transform.position.y, translationZ);
	}
}
