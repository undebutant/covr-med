using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public ObjectDrag objectDrag;

    public Camera avatarCamera;

    float zPosition;
    Vector3 offset;
    Vector3 cursorPosition;
    public float speed;

    // Use this for initialization
    void Start () {
        zPosition = transform.position.z;
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 handScreenPoint = avatarCamera.WorldToScreenPoint(transform.position);

        cursorPosition = handScreenPoint;
        
    }


    // Update is called once per frame
    void Update () {
        if(objectDrag.controllerOn) {
            /*Vector3 handScreenPoint = avatarCamera.WorldToScreenPoint(transform.position);

            cursorPosition = handScreenPoint;*/

            cursorPosition.x = cursorPosition.x + Input.GetAxis("HorizontalDpad")*Time.deltaTime * speed ;

            cursorPosition.y = cursorPosition.y + Input.GetAxis("VerticalDpad") * Time.deltaTime * speed;
            cursorPosition.z = 0.666f;
            offset = transform.position - avatarCamera.ScreenToWorldPoint(cursorPosition);
            transform.position = avatarCamera.ScreenToWorldPoint(cursorPosition + offset);


        } else {
            cursorPosition = Input.mousePosition;
            cursorPosition.z = 0.666f;
            offset = transform.position - avatarCamera.ScreenToWorldPoint(cursorPosition);
            transform.position = avatarCamera.ScreenToWorldPoint(cursorPosition + offset);
        }
        
        

    }
}
