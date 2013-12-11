using UnityEngine;
using System.Collections;

public class CommonGui : MonoBehaviour 
{


	public AudioClip Click1;
	public AudioClip Click2;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	protected void _loadAllAudioClips()
	{
		this.Click1 = this._loadAudioClip("GUI/click1");
		this.Click2 = this._loadAudioClip("GUI/click2");
	}


	protected AudioClip _loadAudioClip(string clipPath)
	{
		return Resources.Load("Sounds/" + clipPath) as AudioClip;
	}
}
