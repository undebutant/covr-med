﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiddleVR_Unity3D;

public class WandSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Position : " + transform.position);
        if (MiddleVR.VRDeviceMgr != null)
        {
            // Getting state of primary wand button
            bool wandButtonPressed0 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(0);

            // Getting toggled state of primary wand button
            // bool wandButtonToggled0 = MiddleVR.VRDeviceMgr.IsWandButtonToggled(0);

            if (wandButtonPressed0)
            {
                // If primary button is pressed, display wand horizontal axis value
                MVRTools.Log("WandButton 0 pressed! HAxis value: " + x + ", VAxis value: " + y + ".");
            }
        }
    }
}
