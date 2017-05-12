using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour {

    public bool[] enableOnScene;
    public bool disable;

    public void AppearOnScene(int index)
    {
        if(!disable)
            gameObject.SetActive(enableOnScene[index]);
    }
	
}
