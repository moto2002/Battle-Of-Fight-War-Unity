using UnityEngine;
using System.Collections;

public class CombatEffects : MonoBehaviour 
{

	private float _startAfterSeconds = 0.0f;
	private float _effectCreationTime = 0.0f;
	private bool _awake = false;


	// Use this for initialization
	void Start () 
	{
		this._effectCreationTime = (float)Time.fixedTime;
		this._startAfterSeconds = Random.Range(0.0f, 6.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!this._awake) {

			if ((float)Time.fixedTime > this._effectCreationTime + this._startAfterSeconds) {

				ParticleSystem CombatEffects = this.gameObject.GetComponent<ParticleSystem>();
				CombatEffects.Play();

				AudioSource SoundSource = this.gameObject.GetComponent<AudioSource>();
				SoundSource.Play();

				this._awake = true;

				//Since we no longer need this shit
				Destroy (this);
			}

		}
	}
}
