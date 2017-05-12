using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour {

    List<SceneObject> _sceneObjectList;
    
    void Start()
    {
        Initialization();
    }

    void Initialization()
    {
        _sceneObjectList = new List<SceneObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            SceneObject sceneObject = transform.GetChild(i).GetComponent<SceneObject>();

            if (sceneObject != null)
                _sceneObjectList.Add(sceneObject);
        }
    }

    public void SceneChanged(int index)
    {
        for (int i = 0; i < _sceneObjectList.Count; i++)
        {
            _sceneObjectList[i].AppearOnScene(index);
        }
    }
}
