using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour {
    
    public string objectName;
    public MeshRenderer meshRenderer;
    
    public List<ObjectData> objectData;

    bool disable = false;
    int index = 0;
    Texture hintIndicator; 

    Renderer renderer;
    GameObject player;

    bool showHint = false;

    [System.Serializable]    
    public struct ObjectData
    {
        public bool enableOnScene;
        public bool interactable;
        public Material[] material;       
    }

    void Start()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if(renderer == null)
            renderer = gameObject.GetComponent<Renderer>();

        if (hintIndicator == null)
            hintIndicator = Resources.Load("UI/WhiteDot", typeof(Texture)) as Texture;
    }

    void Update()
    {
        ShowIndicator();
    }

    public string GetName()
    {
        return objectName;
    }

    public bool IsInteractable()
    {
        return objectData[index].interactable;
    }

    public void ShowIndicator()
    {
        if (objectData[index].interactable && renderer.isVisible && Vector3.Distance(player.transform.position, transform.position) < 7)
        {
            showHint = true;
        }
        else
            showHint = false;
    }

    public void DisableObject()
    {
        gameObject.SetActive(true);
        disable = true;
    }

    public bool IsDisable()
    {
        return disable;
    }

    public void AppearOnScene(int index)
    {
        this.index = index;

        if (!disable)
        {
            gameObject.SetActive(objectData[index].enableOnScene);

            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                if (i <= objectData[index].material.Length - 1)
                    meshRenderer.materials[i] = objectData[index].material[i];                
            }
        }
    }    
}
