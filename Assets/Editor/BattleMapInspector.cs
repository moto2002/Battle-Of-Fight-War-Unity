using UnityEngine;
using UnityEditor;

using System.Collections;

//Type must match game object name
//This tells it to appear on that game object type
[CustomEditor(typeof(BattleMap))]

public class BattleMapInspector : Editor 
 {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Regenerate Map")) {
			BattleMap MyMap = (BattleMap)this.target;
			MyMap.buildMesh();
		}
	}
}
