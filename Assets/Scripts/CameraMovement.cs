using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float translationX = Input.GetAxis ("Horizontal");
		float translationZ = Input.GetAxis ("Vertical");

		translationX *= Time.deltaTime;

		//z is special since we're at an angle
		translationZ *= Time.deltaTime;
		float rotationZInRad = (this.transform.rotation.z * 3.14f)/180.0f;
		translationZ = Mathf.Cos (rotationZInRad) * translationZ;
	
		this.transform.position += transform.right * 10.0f * translationX;
		this.transform.position += transform.forward * 10.0f * translationZ;

		//this.transform.Translate(translationX, this.transform.position.y, translationZ);
	}
}
