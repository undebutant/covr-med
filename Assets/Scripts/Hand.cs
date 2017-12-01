using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    float zPosition;
    Vector3 offset;
    Vector3 cursorPosition;

    // Use this for initialization
    void Start () {
        zPosition = transform.position.z;
        Cursor.visible = false;

    }


    // Update is called once per frame
    void Update () {
        cursorPosition = Input.mousePosition;
        cursorPosition.z = zPosition;

        offset = transform.position - Camera.main.ScreenToWorldPoint(cursorPosition);
        this.transform.position = Camera.main.ScreenToWorldPoint(cursorPosition +offset);

    }
}
