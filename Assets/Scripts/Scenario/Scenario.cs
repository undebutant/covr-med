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

    [Tooltip("If true, the object must be close to achieve the step")]
    public bool isCloseCondition;

    [Tooltip("Instruction to show while the step is not achieved")]
    public String instruction;
}

public class Scenario : MonoBehaviour {
    [SerializeField]
    Step[] steps;

    int currentStepIndex;

    //  Displayer
    [SerializeField]
    [Tooltip("The text component that displays the information for the surgeon")]
    Text displayer;

    // The config for the local instance
    ConfigInitializer config;

    bool IsClose() {
        Debug.Log(steps[currentStepIndex].zone.name);
        return Vector3.Distance(steps[currentStepIndex].selectableObject.transform.position, steps[currentStepIndex].zone.transform.position) < 0.1;
    }


    // Use this for initialization
    void Start() {
        currentStepIndex = 0;
        config = GameObject.FindObjectOfType<ConfigInitializer>();
        // Only surgeons can see the instructions
        if (config.GetPlayerRole() == PlayerRole.Surgeon) {
            // Show the first instructions
            displayer.text = steps[currentStepIndex].instruction;
        } else {
            displayer.text = "Please follow the surgeon's vocal instructions";
        }
    }

    void Update() {
        if (config.GetPlayerRole() == PlayerRole.Surgeon) {
            if (steps[currentStepIndex].isCloseCondition == IsClose()) {
                currentStepIndex++;
                displayer.text = steps[currentStepIndex].instruction;
            }
        }
    }
}
