using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePlacer : MonoBehaviour {
    
    public GameObject[] scenes;
    public Vector3 initialPos = Vector3.zero;
    public float distance;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ArrangeScenes()
    {
        for (int i = 0; i < scenes.Length; i++)        
            scenes[i].transform.position = new Vector3(initialPos.x, distance * i, initialPos.z);        
    }    
}
