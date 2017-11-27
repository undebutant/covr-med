using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour {

    bool modeCameraOn = true;


    public bool isCameraModeOn() {
        return modeCameraOn;
    }

    public bool isSelectModeOn() {
        return !modeCameraOn;
    }

    // Use this for initialization
    void Start () {
        modeCameraOn = true;

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Switch")) {
            modeCameraOn = !modeCameraOn;
        }

    }
}
