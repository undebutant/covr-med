using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

[System.Serializable]
public struct Step {
    public GameObject zone;
    public GameObject selectableObject;
    public String instruction;
}

public class Scenario : MonoBehaviour {
    [SerializeField]
    Step[] steps;

    int currentStepIndex;

    //  Displayer

    [SerializeField]
    Text displayer;



    // Use this for initialization
    void Start() {
        currentStepIndex = 0;
        // Show the first instructions
        displayer.text = steps[currentStepIndex].instruction;
    }

    void Update() {
        if (Vector3.Distance(steps[currentStepIndex].selectableObject.transform.position, steps[currentStepIndex].zone.transform.position) < 0.1) {
            currentStepIndex++;
            displayer.text = steps[currentStepIndex].instruction;
        }
    }
}
