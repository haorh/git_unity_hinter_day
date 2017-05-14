using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneObjectManager : MonoBehaviour {

    List<SceneObject> _sceneObjectList;
    List<SceneObjectData> _sceneObjectData;

    void Start()
    {
        Initialization();        
    }

    void Initialization()
    {
        _sceneObjectList = new List<SceneObject>();
        _sceneObjectData = new List<SceneObjectData>();
        for (int i = 0; i < transform.childCount; i++)
        {
            SceneObject sceneObject = transform.GetChild(i).GetComponent<SceneObject>();

            if (sceneObject != null)
            {
                _sceneObjectList.Add(sceneObject);
                _sceneObjectData.Add(sceneObject.objectData);
            }
        }
    }

    public void SceneChanged(int index)
    {
        for (int i = 0; i < _sceneObjectList.Count; i++)
        {
            _sceneObjectList[i].AppearOnScene(index);
        }
    }

    public List<SceneObject> GetSceneObjects()
    {
        return _sceneObjectList;
    }
}
