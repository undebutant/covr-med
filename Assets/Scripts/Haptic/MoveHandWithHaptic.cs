using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     Script dedicated to make a GameObject follow the haptic arm movement
/// </summary>
public class MoveHandWithHaptic : MonoBehaviour {

    [SerializeField]
    [Tooltip("The controller script of the haptic arm")]
    public HapticManager hapticManager;


	void Start () {
        transform.position = hapticManager.HandPosition;
    }


	void Update () {
      
        // Move the GameObject according to the haptic arm
        transform.position = hapticManager.HandPosition;
        // Rotate the GameObject according to the haptic arm
        transform.rotation = hapticManager.HandRotation;
    }
}
