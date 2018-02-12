using UnityEngine;
using System.Collections;

public class InteractionWandJoint : MonoBehaviour {

    private RaycastHit hitInfo;
    private GameObject objectToMove;
    private float distance_centre_objet;
    private bool isHand = true;
    private GameObject selectedObject;
    private bool hasSelected = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //If the cube with the raycast is used
        if (!isHand)
        {
            //If an object was selected
            if (hasSelected)
            {
                //If the user left clicks (or uses the Wiimote's B button
                if (Input.GetMouseButtonDown(0))
                {
                    //If an object with a collider has been hit but it is not the same as the selected object, or if there is not collider hit, the MoveObjectWithArrows script of the selected object is disabled
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo))
                    {
                        if (!hitInfo.collider.gameObject.Equals(selectedObject))
                        {
                            selectedObject.GetComponent<MoveObjectWithArrows>().enabled = false;
                        }
                    }
                    else if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo))
                    {
                        selectedObject.GetComponent<MoveObjectWithArrows>().enabled = false;
                    }
                }
            
            }

            //Whenever the user presses the left or right click (or B or A button of the wiimote), the LineRenderer's joint is destroyed
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                this.GetComponentInChildren<InteractionJoint>().DestroyJoin();
            }

            //If an object with a collider has been hit by the Linerenderer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo))
            {
                GameObject objectHit = hitInfo.collider.gameObject;
                float distance = hitInfo.distance;
                //If the wand is close enough to the hit object 
                if (distance <= 15.0f)
                {
                    // Debug.Log("Distance ok");
                    //If this object has a MoveObjectWithArrows component, if the user left clicks then the MoveObjectWithArrows component is enabled
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("click");
                        if (objectHit.GetComponent<MoveObjectWithArrows>())
                        {
                            //Debug.Log("Component ok");
                            selectedObject = objectHit;
                            objectHit.GetComponent<MoveObjectWithArrows>().enabled = true;
                            hasSelected = true;
                        }
                    }
                    //If the object has an OpenCloseDoor component the MoveDoor() method is called
                    if (objectHit.GetComponent<OpenCloseDoor>())
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            objectToMove = objectHit;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            objectToMove.GetComponent<OpenCloseDoor>().MoveDoor();
                        }
                    }
                    //Same with Drawer
                    if (objectHit.GetComponent<OpenCloseDrawer>())
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            objectToMove = objectHit;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            objectHit.GetComponent<OpenCloseDrawer>().MoveDrawer();
                        }
                    }
                    //If the object hit has the "Manipulable" tag, then a joint is created between it and the LineRenderer (see InteractionJoin script)
                    if (objectHit.CompareTag("Manipulable"))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            this.GetComponentInChildren<InteractionJoint>().Join(objectHit);
                        }
                    }
                    //If the object hit has the "Selectionnable" tag, then it is teleported to the position slightly forward of the origin of the LineRenderer and a joint is created
                    if (objectHit.CompareTag("Selectionnable"))
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            objectHit.transform.parent.GetComponent<Transform>().position = this.transform.position + 0.4f * this.transform.forward;
                            objectHit.transform.parent.GetComponent<Transform>().rotation = this.transform.rotation;
                            this.GetComponentInChildren<InteractionJoint>().Join(objectHit);
                        }
                    }
                }
            }
        }
        //When the C key (or the C button of the nunchuk) is pressed, the wand becomes a virtual hand (Hand activated and Cube desactivated or the opposite)
        if (Input.GetKeyDown(KeyCode.C))
        {
            isHand = !isHand;
            if (isHand)
            {
                transform.FindChild("Hand").gameObject.SetActive(true);
                transform.FindChild("Cube").gameObject.SetActive(false);
            }
            else
            {
                transform.FindChild("Hand").gameObject.SetActive(false);
                transform.FindChild("Cube").gameObject.SetActive(true);
            }
        }
    }
}
