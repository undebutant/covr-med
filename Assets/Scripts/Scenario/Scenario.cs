using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;


/// <summary>
///     The basic structure for each considered step
/// </summary>
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

    // The GameObject holding the text component we went to update
    [SerializeField]
    [Tooltip("The text component that displays the information for the surgeon")]
    Text displayer;

    // The config for the local instance
    ConfigInitializer config;

    bool IsCloseEnoughToSnapZone() {
        return Vector3.Distance(steps[currentStepIndex].selectableObject.transform.position, steps[currentStepIndex].zone.transform.position) < 0.1;
    }


    void Start() {
        currentStepIndex = 0;
        config = GameObject.FindObjectOfType<ConfigInitializer>();

        // Only the surgeon can see the instructions
        if (config.GetPlayerRole() == PlayerRole.Surgeon) {
            // Show the first instructions
            displayer.text = steps[currentStepIndex].instruction;
        } else {
            displayer.text = "Please follow the surgeon's vocal instructions";
        }
    }


    void Update() {
        if (config.GetPlayerRole() == PlayerRole.Surgeon) {
            if (steps[currentStepIndex].isCloseCondition == IsCloseEnoughToSnapZone()) {
                currentStepIndex++;
                displayer.text = steps[currentStepIndex].instruction;
            }
        }
    }
}
