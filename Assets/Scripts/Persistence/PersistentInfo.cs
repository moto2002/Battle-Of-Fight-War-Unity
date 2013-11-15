using UnityEngine;
using System.Collections;
using System.IO;


public class PersistentInfo : MonoBehaviour 
{
	
	public TextAsset SceneOrder;
	
	public int sceneIndex = 0;
	
	public TextAsset InterludeText;
	
	
	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	public void loadSceneAtIndex(int index)
	{
		this.setSceneIndex(index);
		
		string[] sceneInfo = this._getSceneInfoAtIndex(index);
		
		Debug.Log("Scene type: " + sceneInfo[0]);
		if (sceneInfo[0] == "Interlude") {
			
			this.InterludeText = Resources.Load("Texts/Interludes/" + sceneInfo[1]) as TextAsset;
			Debug.Log ("Texts/Interludes/" + sceneInfo[1]);
			Application.LoadLevel("Interlude");
			
		} else { //Map... load the right scene
			
			Debug.Log(sceneInfo[1]);
			Application.LoadLevel(sceneInfo[1]);
		}
	}
	
	
	private string[] _getSceneInfoAtIndex(int index)
	{
		//Generate list of names
		string sceneList = this.SceneOrder.text;
		StringReader Reader = new StringReader(sceneList);
		
	    string sceneInfo = "";	
		for (int i = 0; i <= index; i++) {
			sceneInfo = Reader.ReadLine();
		}
			
		Debug.Log ("Scene Info :" + sceneInfo);
		
		return sceneInfo.Split(null);
	}
	
	
	public void setSceneIndex(int newIndex)
	{
		this.sceneIndex = newIndex;	
	}
	
	
	public int getSceneIndex()
	{
		return this.sceneIndex;	
	}
}
