using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public CameraDistortion timeWarpEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}        

    void TimeWarp()
    {
        if (Input.GetMouseButtonDown(0) && timeWarpEffect.GetTimeWarpState() == CameraDistortion.State.idle)
            timeWarpEffect.PlayWarpEffect();
    }
}
