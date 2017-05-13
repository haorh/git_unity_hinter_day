using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RaycastScript : MonoBehaviour
{
    public bool raycastInfinite = false;
    public float raycastDistance = 100.0f;

    public enum RaycastMethod { FromCentre, FromMouse };
    public RaycastMethod raycastMethod = RaycastMethod.FromCentre;

    public List<RaycastHit> prevHitInfo;

    public LayerMask collisionLayer;
    Ray ray;

    //---- UI
    public GameObject interactiveIndicator;
    public Text indicatorText;

    //--- incase there is multiple object in one raycast
    Vector3 prevPosition, prevDirection;
    public float updateSecond = 2;
    public bool inCoroutine = false;
    public int lastChosenIndex = 0;
    public bool cameraMoved = false, oneItem;
    public GameObject prevObject;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hitInfo;
        GameObject currentObject;
        Vector3 currentPosition;
        Vector3 forward;

        if (raycastMethod == RaycastMethod.FromCentre)
        {
            currentPosition = Camera.main.transform.position;
            forward = Camera.main.transform.forward;
        }
        else
        {
            currentPosition = Input.mousePosition;
            forward = Camera.main.transform.forward;
        }

        ray = new Ray(currentPosition, forward);

        if (raycastInfinite)
            hitInfo = Physics.RaycastAll(ray, raycastDistance, collisionLayer);
        else
            hitInfo = Physics.RaycastAll(ray, Mathf.Infinity, collisionLayer);


        CheckForCondition(currentPosition, forward);

        ChooseOneObj(hitInfo, out currentObject, currentPosition);
        DeactivatePrevObj(currentObject);
        ActivateObj(currentObject);
        UpdateObj(currentObject);
    }

    void ChooseOneObj(RaycastHit[] hitInfo, out GameObject currentObj, Vector3 currentPosition)
    {
        currentObj = null;
        if (hitInfo.Length == 1)
        {
            currentObj = hitInfo[0].collider.gameObject;
            oneItem = true;
            //Debug.Log("Hit one");
        }
        else if (hitInfo.Length > 1)
        {
            
            if (!inCoroutine)
            {
                lastChosenIndex = 0;
                prevPosition = currentPosition;
                StartCoroutine(ChooseAmongList(hitInfo.Length));
            }          
                

            oneItem = false;            
            currentObj = hitInfo[Mathf.Min(lastChosenIndex, hitInfo.Length - 1)].collider.gameObject;
            //Debug.Log("Hit multiple");
        }
    }

    void DeactivatePrevObj(GameObject currentObj)
    {
        if (currentObj == prevObject)
            return;
        if (prevObject == null)
            return;

        if (prevObject.GetComponent<HighlightItem>())
            prevObject.GetComponent<HighlightItem>().Unhighlight();

        if (prevObject.GetComponent<PickableItem>())
            prevObject.GetComponent<PickableItem>().Undetected();

        UpdateUI(false, null);
    }

    void ActivateObj(GameObject currentObj)
    {
        if (currentObj == prevObject)
            return;
        if (currentObj == null)
            return;

        if (currentObj.GetComponent<HighlightItem>())
            currentObj.GetComponent<HighlightItem>().Highlight();

        if (currentObj.GetComponent<PickableItem>())
            currentObj.GetComponent<PickableItem>().Detected();

        UpdateUI(true, currentObj);
    }

    void UpdateObj(GameObject currentObj)
    {
        prevObject = currentObj;
    }

    void UpdateUI(bool show, GameObject currentObj)
    {
        if(show)
        {
            interactiveIndicator.SetActive(true);
            var pos = Camera.main.WorldToScreenPoint(currentObj.transform.position);
            interactiveIndicator.transform.position = pos;
            indicatorText.text = currentObj.name;
        }
        else
        {
            interactiveIndicator.SetActive(false);
        }
    }

    void DeactivatePrevList(RaycastHit[] hitInfo)
    {
        List<int> index = new List<int>();

        for (int x = 0; x < prevHitInfo.Count; x++)
        {
            var found = false;
            for (int y = 0; y < hitInfo.Length; y++)
            {
                if (prevHitInfo[x].collider.gameObject == hitInfo[x].collider.gameObject)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var obj = prevHitInfo[x].collider.gameObject;
                if (obj.GetComponent<HighlightItem>())
                    obj.GetComponent<HighlightItem>().Unhighlight();

                if (obj.GetComponent<PickableItem>())
                    obj.GetComponent<PickableItem>().Undetected();
            }
        }
    }

    void ActivateList(RaycastHit[] hitInfo)
    {
        for (int x = 0; x < hitInfo.Length; x++)
        {
            var obj = hitInfo[x].collider.gameObject;
            if (obj.GetComponent<HighlightItem>())
                obj.GetComponent<HighlightItem>().Highlight();

            if (obj.GetComponent<PickableItem>())
                obj.GetComponent<PickableItem>().Detected();
        }
    }

    void UpdateList(RaycastHit[] hitInfo)
    {
        prevHitInfo = hitInfo.ToList();
    }

    IEnumerator ChooseAmongList(int length)
    {
        inCoroutine = true;
        float timer = 0.0f;
        while(true)
        {
            timer += Time.deltaTime;
            if (timer > updateSecond && cameraMoved)
            {
                lastChosenIndex++;
                if (lastChosenIndex > length)
                    lastChosenIndex = 0;

                timer = 0;
            }
            else if (oneItem)//if there is only one item collided, break      
            {
                Debug.Log("Brokw coroutine");
                break;
            }
            yield return null;
        }
        inCoroutine = false;
    }
    void CheckForCondition(Vector3 currentPosition, Vector3 direction)
    {
        if (prevPosition != currentPosition || prevDirection != direction)
            cameraMoved = true;
        else
            cameraMoved = false;

        prevDirection = direction;
        prevPosition = currentPosition;
    }
}
