using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public ObjectDrag objectDrag;

    float zPosition;
    Vector3 offset;
    Vector3 cursorPosition;
    public float speed;

    // Use this for initialization
    void Start () {
        zPosition = transform.position.z;
        Cursor.visible = false;

    }


    // Update is called once per frame
    void Update () {
        if(objectDrag.controllerOn) {
            Vector3 handScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

            cursorPosition = handScreenPoint;

            cursorPosition.x = cursorPosition.x + Input.GetAxis("HorizontalDpad")*Time.deltaTime * speed ;

            cursorPosition.y = cursorPosition.y + Input.GetAxis("VerticalDpad") * Time.deltaTime * speed;

            offset = transform.position - Camera.main.ScreenToWorldPoint(cursorPosition);
            transform.position = Camera.main.ScreenToWorldPoint(cursorPosition + offset);


        } else {
            cursorPosition = Input.mousePosition;
            
            offset = transform.position - Camera.main.ScreenToWorldPoint(cursorPosition);
            transform.position = Camera.main.ScreenToWorldPoint(cursorPosition + offset);
        }
        

    }
}
