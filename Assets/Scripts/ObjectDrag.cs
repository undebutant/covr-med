﻿using System;
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

  


    // Movement variables
    bool isDragFeatureOn;
    GameObject deviceSelector;
    GameObject objectSelected;
    float distance;


    // Snap variables
    GameObject zone;                                // The area to touch with the object for the snap feature
    public float closeDistance = 1.0f;              // The maximum range between the snap zone and the object for the snap to work
    Color closeColor = new Color(0, 1, 0);          // The color of the area whenever a dragged object is nearby
    private Color normalColor = new Color();

    public bool getIsDragFeatureOn () {
        return isDragFeatureOn;
    }

    void Start () {
        // Finding the snap area and storing it
        zone = GameObject.FindGameObjectWithTag("Zone");
        normalColor = zone.GetComponent<Renderer>().material.color;
        isDragFeatureOn = false;

    }

    public void selectObject(GameObject newdeviceSelector, GameObject newobjectSelected, float newdistance) {
        if(newdeviceSelector != null && newobjectSelected != null) {
            deviceSelector = newdeviceSelector;
            objectSelected = newobjectSelected;
            distance = newdistance;
            isDragFeatureOn = true;
            Debug.LogError("ON");
        } else {
            isDragFeatureOn = false;
            throw new Exception("BAD device Selector or BAD object to select");
        }
        
    }

    void moveObject() {
        if (isDragFeatureOn) {

            Vector3 newPos = deviceSelector.transform.position;
            Quaternion newRot = deviceSelector.transform.rotation;

            Vector3 zonePosition = zone.transform.position;

            // Calling the synchronise online method to propagate the movement
            // THIS IS THE DIFFICULT PART OF THE UNITY NETWORK, see associated script for more infos
            playerMoveObject.moveObject(objectSelected, newPos, newRot);

            /*
            // Checking whether or not the object is close enough to highlight the snap zone
            float distance = Vector3.Distance(zonePosition, objectSelected.transform.position);
            zone.GetComponent<Renderer>().material.color = (distance < closeDistance) ? closeColor : normalColor;*/
        }
    }


    public void releaseObject() {
        if (isDragFeatureOn) {

            Vector3 zonePosition = zone.transform.position;

            // Checking whether or not the object is close enough to snap the object
            float distance = Vector3.Distance(zonePosition, objectSelected.transform.position);
            if (distance < closeDistance) {
                Vector3 newPos = zone.transform.position;

                // Fix needed when the transform is not at the bottom of the object
                // Fix done for the cube
                if (objectSelected.CompareTag("Cube")) {
                    newPos = newPos + new Vector3(0, objectSelected.transform.lossyScale.y / 2.0f, 0);
                }

                // Calling the synchronise online method to propagate the movement
                // THIS IS THE DIFFICULT PART OF THE UNITY NETWORK, see associated script for more infos
                playerMoveObject.moveObject(objectSelected, newPos, zone.transform.rotation);
            }

            // Resetting the color since the object is no longer held
            zone.GetComponent<Renderer>().material.color = normalColor;
        }
        deviceSelector = null;
        objectSelected = null;
        isDragFeatureOn = false;
        Debug.LogError("OFF");
    }



	void Update () {
        moveObject();
    } 
}
