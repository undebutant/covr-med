using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     Script allowing snap, drag and drop for specific objects in the scene
///     Objects need to have a NetworkIdentity, to be in the "selectionable" layer,
///     and to have a collider for the raycast
/// </summary>
public class ObjectDrag : MonoBehaviour {

    [SerializeField]
    [Tooltip("The network script attached in order to synchronise the drag and drop online")]
    PlayerMoveObject playerMoveObject;

    // TODO use network menu to choose the controller
    [SerializeField]
    [Tooltip("Indicates if the player is using a controller")]
    Boolean controllerOn;

    [SerializeField]
    [Tooltip("The camera linked to the player avatar")]
    Camera avatarCamera;

    // Raycast parameters
    RaycastHit shootHit;

    [SerializeField]
    [Tooltip("The max range for the selection raycast")]
    int range = 1000;

    // Storing the layer mask of the selectionable layer as an int
    int layerSelectionable;


    // Movement variables
    bool isDragFeatureOn;
    Vector3 offset;                                 // The distance offset between the camera and the hand - TODO make hand and camera independant
    Vector3 objectScreenPoint;                      // The projection of the object on the player screen
    float zOrigin;


    // Snap variables
    GameObject zone;                                // The area to touch with the object for the snap feature
    public float closeDistance = 1.0f;              // The maximum range between the snap zone and the object for the snap to work
    Color closeColor = new Color(0, 1, 0);          // The color of the area whenever a dragged object is nearby
    private Color normalColor = new Color();



    void Start () {
        // Finding the snap area and storing it
        zone = GameObject.FindGameObjectWithTag("Zone");
        normalColor = zone.GetComponent<Renderer>().material.color;

        // Storing the selectionable layer
        layerSelectionable = LayerMask.NameToLayer("selectionable");

        isDragFeatureOn = false;
    }
	

    void callRayCast(Vector3 handScreenPoint) {
        Ray ray = avatarCamera.ScreenPointToRay(avatarCamera.WorldToScreenPoint(transform.position));

        // Whenever the rayCast hits something...
        if (Physics.Raycast(ray, out shootHit, range)) {
            // ... matching the layer
            if (shootHit.collider.gameObject.layer == layerSelectionable) {
                isDragFeatureOn = true;

                objectScreenPoint = avatarCamera.WorldToScreenPoint(shootHit.collider.gameObject.transform.position);
                offset = shootHit.collider.gameObject.transform.position - avatarCamera.ScreenToWorldPoint(new Vector3(handScreenPoint.x, handScreenPoint.y, objectScreenPoint.z));

                // Resetting rotation of object selected
                shootHit.collider.gameObject.transform.rotation = Quaternion.identity;
                zOrigin = 0;
            }
        }
    }

    void moveObject(Vector3 handScreenPoint) {
        if (isDragFeatureOn) {

            if (Input.GetButton("HandFront")) {
                zOrigin -= Time.deltaTime;
                if(objectScreenPoint.z + zOrigin< handScreenPoint.z - 0.20) {
                    zOrigin = handScreenPoint.z  - 0.20f - objectScreenPoint.z;
                }
            }

            if (Input.GetButton("HandBack")) {
                zOrigin += Time.deltaTime;

                if (objectScreenPoint.z + zOrigin > handScreenPoint.z +0.40) {
                    zOrigin = handScreenPoint.z +0.40f - objectScreenPoint.z;
                }
            }


            Vector3 curScreenPoint = new Vector3(handScreenPoint.x, handScreenPoint.y, objectScreenPoint.z + zOrigin);
            Vector3 curPosition = avatarCamera.ScreenToWorldPoint(curScreenPoint) + offset;
            Vector3 zonePosition = zone.transform.position;

            //shootHit.collider.gameObject.transform.position = curPosition;

            playerMoveObject.moveObject(shootHit.collider.gameObject, curPosition, shootHit.collider.gameObject.transform.rotation);

            float distance = Vector3.Distance(zonePosition, shootHit.collider.gameObject.transform.position);
            zone.GetComponent<Renderer>().material.color = (distance < closeDistance) ? closeColor : normalColor;

        }
    }

    void releaseObject() {
        if (isDragFeatureOn) {
            Vector3 zonePosition = zone.transform.position;
            float distance = Vector3.Distance(zonePosition, shootHit.collider.gameObject.transform.position);
            if (distance < closeDistance) {
                
                Vector3 newPos = zone.transform.position ;

                if(shootHit.collider.gameObject.CompareTag("Cube")) {
                    newPos = newPos + new Vector3(0, shootHit.collider.gameObject.transform.lossyScale.y / 2.0f, 0);
                }

                playerMoveObject.moveObject(shootHit.collider.gameObject, newPos, zone.transform.rotation);

                zone.GetComponent<Renderer>().material.color = normalColor;
            } else {
                zone.GetComponent<Renderer>().material.color = normalColor; //le bug fix avant le bug
            }
        }
        isDragFeatureOn = false;
    }

	// Update is called once per frame
	void Update () {
        Vector3 handScreenPoint = avatarCamera.WorldToScreenPoint(transform.position);
        if(controllerOn) {
            if (Input.GetButtonDown("Fire1")) {
                if(!isDragFeatureOn) {
                    callRayCast(handScreenPoint);
                } else {
                    releaseObject();
                }
            }

            moveObject(handScreenPoint);
        } else {
            if (Input.GetButtonDown("Fire1")) {
                callRayCast(handScreenPoint);
            }

            if (Input.GetButtonUp("Fire1")) {
                releaseObject();
            }

            moveObject(handScreenPoint);
        }
    } 
}
