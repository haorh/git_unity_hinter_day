using UnityEngine;
using UnityEngine.UI;

public class RaycastObject : MonoBehaviour {

    public Transform camera;
    public GameObject cursor;
    public GameObject interactiveIndicator;
    public Text indicatorText;
    public float raycastDistance;

    RaycastHit hit;    
    Ray ray;
    bool objectDetected = false;
    SceneObject sceneObject = null;
    Vector2 pos = Vector2.zero;

    void Start()
    {
        
    }

    void Update () {        
        RaycastToMiddle();
	}

    void RaycastToMiddle()
    {
        ray = new Ray(camera.position, camera.forward);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            objectDetected = true;
            sceneObject = hit.collider.gameObject.GetComponent<SceneObject>();
            if (sceneObject != null)
            {
                if (sceneObject.IsInteractable())
                {
                    interactiveIndicator.SetActive(true);
                    //cursor.SetActive(false);
                    pos = Camera.main.WorldToScreenPoint(hit.collider.gameObject.transform.position);
                    interactiveIndicator.transform.position = pos;
                    indicatorText.text = sceneObject.GetName();
                }
            }
        }
        else
        {
            //cursor.SetActive(true);
            interactiveIndicator.SetActive(false);
            objectDetected = false;
            sceneObject = null;
        }
    }    
}
