using UnityEngine;
using System.Collections;

public class Sunlight : MonoBehaviour 
{
	
	
	private LevelInfo _LevelInformations;
	
	private Color _Night = new Color(8.0f/255.0f, 50.0f/255.0f, 100.0f/255.0f);
	
	
	// Use this for initialization
	void Start () 
	{
		this._LevelInformations = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	
	
	void FixedUpdate()
	{
		//return;
		int battleTime = (this._LevelInformations.battleTime % 1440);
		
		if (battleTime >= 480 && battleTime <= 1080) { //daylight
			
			this.light.color = Color.white;
			
		} else if (battleTime > 1080 && battleTime <= 1260) { //sunset
			
			if (battleTime <= 1170) { //transition from day to red
				float difference = (1170.0f - battleTime) / 90.0f;
				this.light.color = new Color(
					(1.0f - difference) + (Color.white.r * difference),
					0.0f + Color.white.g * difference,
					0.0f + Color.white.b * difference
				);
			} else { //between 1170 and 1260; transition from red to night
				float difference = (1260.0f - battleTime) / 90.0f;
				this.light.color = new Color(
					(this._Night.r * (1.0f - difference) + (difference)),
					this._Night.g * (1.0f - difference),
					this._Night.b * (1.0f - difference)
				);
			}
						
		} else if ((battleTime > 1260 && battleTime <= 1440) || (battleTime >= 0 && battleTime <= 300)) { //night
				
			this.light.color = this._Night;
			
		} else { //sunrise - between 300 and 480
			
			if (battleTime >= 300 && battleTime < 390) { //transition from night to red
				float difference = (390.0f - battleTime) / 90.0f;
				this.light.color = new Color(
					(1.0f - difference) + (this._Night.r * difference),
					0.0f + this._Night.g * difference,
					0.0f + this._Night.b * difference
				);
				
			} else { //time b/t 390 and 480; transition from red to white
				float difference = (480.0f - battleTime) / 90.0f;
				this.light.color = new Color(
					(1.0f - difference) + (Color.red.r * difference),
					(1.0f - difference),
					(1.0f - difference)
				);
			}
		}
		
		//Debug.Log("r=" + this.light.color.r + ", g=" + this.light.color.g + ", b=" + this.light.color.b);
	}
	
	
}
