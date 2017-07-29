using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightItem : MonoBehaviour {
    public enum HighLightMethod{ ChangeColour,HighLightBorder};
    public HighLightMethod highlightMethod = HighLightMethod.ChangeColour;

    //public Color highlightColour = Color.green;
    //Color defaultColour = Color.white;

    [Header("Data, should be private")]
    public bool detected;
    public bool detectThisFrame;

    [Header("Debug Purpose")]
    public GUIText debugText;
    // Use this for initialization
    void Start () {
		
	}
    // void Update()
    //{
    //    detectThisFrame = false;
    //}
    //// Update is called once per frame
    //void LateUpdate () {
    //    if (!detectThisFrame)
    //        Unhighlight();
    //}

    public void Highlight()
    {
        detectThisFrame = true;

        detected = true;
        //Debug.Log("Raycast hit this obj = " + gameObject.name);
    }

    public void Unhighlight()
    {
        detected = false;
    }
}
