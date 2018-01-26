using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     Script allowing snap, drag and drop for specific objects in the scene.
///     Objects need to have a NetworkIdentity, to be in the "selectionable" layer,
///     and to have a collider for the raycast.
/// </summary>
public class ObjectDrag : MonoBehaviour {

    [SerializeField]
    [Tooltip("The network script attached in order to synchronise the drag and drop online")]
    PlayerMoveObject playerMoveObject;

    // TODO use network menu to choose the controller
    [Tooltip("Indicates if the player is using a controller")]
    public Boolean controllerOn;

    [SerializeField]
    [Tooltip("The camera linked to the player avatar")]
    Camera avatarCamera;


    // Raycasts parameters
    RaycastHit shootHit;                        // On click rayCast
    RaycastHit shootHitHL;                      // Passiv rayCast for highlighting

    [SerializeField]
    [Tooltip("The max range for the selection raycast")]
    int range = 1000;

    [SerializeField]
    [Tooltip("The shader to apply on the object whenever a passiv raycast hit")]
    Shader shaderHighLight;
    Shader shaderNormal;

    GameObject lastGameObjectHitByRaycast;

    // Storing the layer mask of the "selectionable" layer as an int
    int layerSelectable;


    // Movement variables
    bool isDragFeatureOn;
    float zOrigin;                                  // The distance between the selected object and the player camera when grabbed
    Vector3 offset;                                 // The distance offset between the camera and the hand - TODO make hand and camera independant
    Vector3 objectScreenPoint;                      // The projection of the object on the player screen

    // Snap variables
    GameObject zone;                                // The area to touch with the object for the snap feature
    public float closeDistance = 1.0f;              // The maximum range between the snap zone and the object for the snap to work
    Color closeColor = new Color(0, 1, 0);          // The color of the area whenever a dragged object is nearby
    private Color normalColor = new Color();



    void Start () {
        // Finding the snap area and storing it
        zone = GameObject.FindGameObjectWithTag("Zone");
        normalColor = zone.GetComponent<Renderer>().material.color;

        // Storing the selectable layer
        layerSelectable = LayerMask.NameToLayer("selectionable");

        isDragFeatureOn = false;
        lastGameObjectHitByRaycast = null;
    }
	

    /// <summary>
    ///     Raycast done regularly to highlight selectable objects
    ///     TODO factorise code with the following method
    /// </summary>
    void callRayCastHighlight () {
        Ray ray = avatarCamera.ScreenPointToRay(avatarCamera.WorldToScreenPoint(transform.position));

        // Whenever the rayCast hits something...
        if (Physics.Raycast(ray, out shootHitHL, range)) {
            // ... matching the layer
            if (shootHitHL.collider.gameObject.layer == layerSelectable) {
               if(lastGameObjectHitByRaycast==null) {
                    // Shading the new object hit to highlight it
                    lastGameObjectHitByRaycast = shootHitHL.collider.gameObject;
                    shaderNormal = lastGameObjectHitByRaycast.GetComponent<Renderer>().material.shader ;
                    lastGameObjectHitByRaycast.GetComponent<Renderer>().material.shader = shaderHighLight;
                } else {
                    if(lastGameObjectHitByRaycast!= shootHitHL.collider.gameObject) {
                        // Shading the previous object back to normal
                        lastGameObjectHitByRaycast.GetComponent<Renderer>().material.shader = shaderNormal;

                        // Shading the new object hit to highlight it
                        lastGameObjectHitByRaycast = shootHitHL.collider.gameObject;
                        shaderNormal = lastGameObjectHitByRaycast.GetComponent<Renderer>().material.shader;
                        lastGameObjectHitByRaycast.GetComponent<Renderer>().material.shader = shaderHighLight;
                    }
                }
            } else {
                if (lastGameObjectHitByRaycast != null) {
                    lastGameObjectHitByRaycast.GetComponent<Renderer>().material.shader = shaderNormal;
                    lastGameObjectHitByRaycast = null;
                }
            }
        } else {
            if (lastGameObjectHitByRaycast != null) {
                lastGameObjectHitByRaycast.GetComponent<Renderer>().material.shader = shaderNormal;
                lastGameObjectHitByRaycast = null;
            }
        }
    }


    /// <summary>
    ///     Raycast called on player click to enable drag mode
    /// </summary>
    void callRayCast(Vector3 handScreenPoint) {
        Ray ray = avatarCamera.ScreenPointToRay(avatarCamera.WorldToScreenPoint(transform.position));

        // Whenever the rayCast hits something...
        if (Physics.Raycast(ray, out shootHit, range)) {
            // ... matching the layer
            if (shootHit.collider.gameObject.layer == layerSelectable) {
				//Find the scenario manager to tell him the object was selected
				//GameObject.Find("ScenarioManager").GetComponent<Scenario>().SetSelectedObject(shootHit.collider.gameObject);
                //TODO FIX NULL POINTER EXCEPTION, DU COUP DRAG MARCHE PAS

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
            // If the player hits the button to pull the object closer
            if (Input.GetButton("HandFront")) {
                zOrigin -= Time.deltaTime;

                // Limiting the minimum distance between the player and the object
                if (objectScreenPoint.z + zOrigin < handScreenPoint.z - 0.20) {
                    zOrigin = handScreenPoint.z  - 0.20f - objectScreenPoint.z;
                }
            }

            // If the player hits the button to push the object farther
            if (Input.GetButton("HandBack")) {
                zOrigin += Time.deltaTime;

                // Limiting the maximum distance between the player and the object
                if (objectScreenPoint.z + zOrigin > handScreenPoint.z +0.40) {
                    zOrigin = handScreenPoint.z +0.40f - objectScreenPoint.z;
                }
            }


            // The vector storing the position of the hand as projected on the screen, with z the output of the object projection on the screen
            Vector3 curScreenPoint = new Vector3(handScreenPoint.x, handScreenPoint.y, objectScreenPoint.z + zOrigin);

            // The in world position of the object, considering the hand x and y coordinates
            Vector3 curPosition = avatarCamera.ScreenToWorldPoint(curScreenPoint) + offset;

            Vector3 zonePosition = zone.transform.position;

            // TODO apply local change immediatly
            //shootHit.collider.gameObject.transform.position = curPosition;

            // Calling the synchronise online method to propagate the movement
            // THIS IS THE DIFFICULT PART OF THE UNITY NETWORK, see associated script for more infos
            playerMoveObject.moveObject(shootHit.collider.gameObject, curPosition, shootHit.collider.gameObject.transform.rotation);


            // Checking whether or not the object is close enough to highlight the snap zone
            float distance = Vector3.Distance(zonePosition, shootHit.collider.gameObject.transform.position);
            zone.GetComponent<Renderer>().material.color = (distance < closeDistance) ? closeColor : normalColor;
        }
    }


    void releaseObject() {
        if (isDragFeatureOn) {
            // Update the scenario manager
            //GameObject.Find("ScenarioManager").GetComponent<Scenario>().UnsetSelectedObject(shootHit.collider.gameObject);
            //TODO FIX NULL POINTER EXCEPTION, DU COUP DRAG MARCHE PAS

            Vector3 zonePosition = zone.transform.position;

            // Checking whether or not the object is close enough to snap the object
            float distance = Vector3.Distance(zonePosition, shootHit.collider.gameObject.transform.position);
            if (distance < closeDistance) {
                Vector3 newPos = zone.transform.position;

                // Fix needed when the transform is not at the bottom of the object
                // Fix done for the cube
                if (shootHit.collider.gameObject.CompareTag("Cube")) {
                    newPos = newPos + new Vector3(0, shootHit.collider.gameObject.transform.lossyScale.y / 2.0f, 0);
                }

                // Calling the synchronise online method to propagate the movement
                // THIS IS THE DIFFICULT PART OF THE UNITY NETWORK, see associated script for more infos
                playerMoveObject.moveObject(shootHit.collider.gameObject, newPos, zone.transform.rotation);
            }

            // Resetting the color since the object is no longer held
            zone.GetComponent<Renderer>().material.color = normalColor;
        }

        isDragFeatureOn = false;
    }



	void Update () {
        Vector3 handScreenPoint = avatarCamera.WorldToScreenPoint(transform.position);

        callRayCastHighlight();

        if (controllerOn) {
            // Toggle drag mode
            if (Input.GetButtonDown("Fire1")) {
                if(!isDragFeatureOn) {
                    callRayCast(handScreenPoint);
                } else {
                    releaseObject();
                }
            }

        } else {
            // Hold drag mode
            if (Input.GetButtonDown("Fire1")) {
                callRayCast(handScreenPoint);
            }

            if (Input.GetButtonUp("Fire1")) {
                releaseObject();
            }
        }

        moveObject(handScreenPoint);
    } 
}
