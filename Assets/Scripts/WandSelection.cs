using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiddleVR_Unity3D;

public class WandSelection : MonoBehaviour {

    [SerializeField]
    private string selectableObjectsLayerName = "selectionable";

    [SerializeField]
    private string buttonObjectsLayerName = "button";

    [SerializeField]
    MainMenuManager mainMenuManager;

    int selectableObjectsLayer;
    int buttonObjectsLayer;

    bool clicked = false;

	// Use this for initialization
	void Start () {
        selectableObjectsLayer =  LayerMask.NameToLayer(selectableObjectsLayerName);
        buttonObjectsLayer = LayerMask.NameToLayer(buttonObjectsLayerName);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (MiddleVR.VRDeviceMgr != null) {
            // Getting state of primary wand button
            bool wandButtonPressed0 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(0);

            // Getting toggled state of primary wand button
            // bool wandButtonToggled0 = MiddleVR.VRDeviceMgr.IsWandButtonToggled(0);

            if (Physics.Raycast(transform.position, fwd, out hit)) {
                if(hit.collider.gameObject.layer == selectableObjectsLayer) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
                    if (wandButtonPressed0) {
                        // Here, call the drag and drop
                        
                    }
                } else if (hit.collider.gameObject.layer == buttonObjectsLayer) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
                    // Click on the button only once
                    if (wandButtonPressed0 && !clicked) {
                        Debug.Log("Button clicked");
                        clicked = true;
                        mainMenuManager.OnHitButton(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}
