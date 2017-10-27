using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engines : MonoBehaviour {

    private Vector3 speed;

    [SerializeField]
    public float MaxSpeed;

    public Vector3 Speed {
        get {
            return this.speed;
        }
        set {
            this.speed = value;
        }
    }

    void Update() {
        transform.Translate(speed * MaxSpeed * Time.deltaTime);
    }
}