using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     Basic movement for a given object, given its maximum speed, and the input
/// </summary>
public class Engines : MonoBehaviour {

    Vector3 inputDirections;

    public Vector3 InputDirection {
        get {
            return this.inputDirections;
        }
        set {
            this.inputDirections = value;
        }
    }


    public float maxSpeed;


    void Update() {
        transform.Translate(inputDirections * maxSpeed * Time.deltaTime);
    }
}
