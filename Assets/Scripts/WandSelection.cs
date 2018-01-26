using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiddleVR_Unity3D;

public class WandSelection : MonoBehaviour {

    [SerializeField]
    private string selectableObjectsLayerName = "selectionable";

    int selectableObjectsLayer;

	// Use this for initialization
	void Start () {
        selectableObjectsLayer =  LayerMask.NameToLayer(selectableObjectsLayerName);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (MiddleVR.VRDeviceMgr != null)
        {
            // Getting state of primary wand button
            bool wandButtonPressed0 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(0);

            // Getting toggled state of primary wand button
            // bool wandButtonToggled0 = MiddleVR.VRDeviceMgr.IsWandButtonToggled(0);

            if (wandButtonPressed0)
            {
                // If primary button is pressed, display wand horizontal axis value
                MVRTools.Log("WandButton 0 pressed!");
            }

            if (Physics.Raycast(transform.position, fwd, out hit)) {
                if(hit.collider.gameObject.layer == selectableObjectsLayer)
                {
                    Debug.Log("Hover the object : " + hit.collider.gameObject.name);
                }
            }
        }
    }
}
