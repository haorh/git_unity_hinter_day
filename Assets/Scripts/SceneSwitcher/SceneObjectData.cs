using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneObjectData{

    public List<ObjectData> objectData;

    [System.Serializable]
    public struct ObjectData
    {
        public bool enableOnScene;
        public bool interactable;
        public Material[] material;
    }
}
