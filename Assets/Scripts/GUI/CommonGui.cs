using UnityEngine;
using System.Collections;

public class CommonGui : MonoBehaviour 
{


	public AudioClip Click1;
	public AudioClip Click2;

	public AudioSource GuiAudioSource; 


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	protected void _initCommonGui()
	{
		this.Click1 = this._loadAudioClip("GUI/click1");
		this.Click2 = this._loadAudioClip("GUI/click2");

		this.GuiAudioSource = this.gameObject.AddComponent<AudioSource>();

		CommonMenuUtilities.CurrentGui = this;
	}


	protected AudioClip _loadAudioClip(string clipPath)
	{
		return Resources.Load("Sounds/" + clipPath) as AudioClip;
	}
}
