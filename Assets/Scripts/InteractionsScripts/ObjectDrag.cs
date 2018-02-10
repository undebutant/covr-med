using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     Script allowing snap, drag and drop for specific objects in the scene.
///     Objects need to have a NetworkIdentity, to be in the "Selectable" layer,
///     and to have a collider for the raycast.
/// </summary>
public class ObjectDrag : MonoBehaviour {

    [SerializeField]
    [Tooltip("The network script attached in order to synchronise the drag and drop online")]
    PlayerMoveObject playerMoveObject;

    // Movement variables
    bool isDragFeatureOn;
    GameObject deviceSelector;
    GameObject objectSelected;

    // The distance between the device and the selected object
    float distance;

    // Snap variables
    GameObject[] zones;                             // The areas to touch with the object for the snap feature
    public float closeDistance;                     // The maximum range between the snap zone and the object for the snap to work

    Color closeColor = new Color(0, 1, 0);          // The color of the area whenever a dragged object is nearby
    private Color normalColor = new Color();

    // The config initialized on startup
    ConfigInitializer configInitializer;


    public bool GetIsDragFeatureOn () {
        return isDragFeatureOn;
    }


    void Start () {
        // Finding all the snap areas in the scene and storing them
        zones = GameObject.FindGameObjectsWithTag("Zone");
        normalColor = zones[0].GetComponent<Renderer>().material.color;

        isDragFeatureOn = false;

        // Fetching the config component
        configInitializer = GameObject.FindObjectOfType<ConfigInitializer>();

        // Allowing snaping from a larger distance when using the cave
        if (configInitializer.GetDisplayDevice() == DisplayDevice.Cave) {
            closeDistance = 0.15f;
        }
        else {
            closeDistance = 0.1f;
        }
    }
    

    /// <summary>
    ///     Public function called by a device to start dragging an object
    /// </summary>
    /// <param name="newdeviceSelector">The Device GameObject</param>
    /// <param name="newobjectSelected">The GameObject to move</param>
    /// <param name="newdistance">The distance to keep between the device and the object</param>
    public void SelectObject(GameObject newdeviceSelector, GameObject newobjectSelected, float newdistance) {
        if(newdeviceSelector != null && newobjectSelected != null) {
            deviceSelector = newdeviceSelector;
            objectSelected = newobjectSelected;
            distance = newdistance;

            isDragFeatureOn = true;

            // Syncing modification online
            playerMoveObject.SyncObjectKinematic(newobjectSelected, true);
        } else {
            isDragFeatureOn = false;

            throw new Exception("Bad device Selector or bad object to select");
        }
    }


    // The function called each frame to move the object
    void TrackSelectedObject() {
        if (isDragFeatureOn) {
            Vector3 newPos = deviceSelector.transform.position + deviceSelector.transform.forward * this.distance;
            Quaternion newRot = deviceSelector.transform.rotation;

            // If the selected object is a syringe, we apply a 180°C rotation on the Z axis, otherwise the syringe would be upside down
            if (objectSelected.CompareTag("Syringe")) {
                newRot = newRot * Quaternion.Euler(0, 0, -90);
            }

            // Calling the synchronise online method to propagate the movement
            // THIS IS THE DIFFICULT PART OF THE UNITY NETWORK, see associated script for more infos
            playerMoveObject.MoveObject(objectSelected, newPos, newRot);

            foreach (GameObject zone in zones) {
                Vector3 zonePosition = zone.transform.position;

                // Checking whether or not the object is close enough to highlight the snap zone considered
                float distance = Vector3.Distance(zonePosition, objectSelected.transform.position);
                zone.GetComponent<Renderer>().material.color = (distance < closeDistance) ? closeColor : normalColor;
            }
        }
    }


    /// <summary>
    /// Public function to call to release the object selected
    /// </summary>
    public void ReleaseObject() {
        if (isDragFeatureOn) {
            foreach (GameObject zone in zones) {
                Vector3 zonePosition = zone.transform.position;

                // Checking whether or not the object is close enough to snap the object to the zone considered
                float distance = Vector3.Distance(zonePosition, objectSelected.transform.position);

                if (distance < closeDistance) {
                    Vector3 newPos = zone.transform.position;
                    Quaternion newRot = zone.transform.rotation;

                    // Fix needed when the transform is not at the bottom of the object
                    // Fix done for the cube
                    if (objectSelected.CompareTag("Cube")) {
                        newPos = newPos + new Vector3(0, objectSelected.transform.lossyScale.y / 2.0f, 0);
                    }

                    // Fix done for the Syringe
                    if (objectSelected.CompareTag("Syringe")) {
                        newRot = newRot * Quaternion.Euler(0, 90, 90);
                    }

                    // Calling the synchronise online method to propagate the movement
                    // THIS IS THE DIFFICULT PART OF THE UNITY NETWORK, see associated script for more infos
                    playerMoveObject.MoveObject(objectSelected, newPos, newRot);
                }
                // Resetting the color since the object is no longer held
                zone.GetComponent<Renderer>().material.color = normalColor;
            } 
        }

        // Syncing modification online
        playerMoveObject.SyncObjectKinematic(objectSelected, false);

        deviceSelector = null;
        objectSelected = null;

        isDragFeatureOn = false;
    }



	void Update () {
        TrackSelectedObject();
    } 
}
