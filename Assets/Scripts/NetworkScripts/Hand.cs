using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Hand : NetworkBehaviour {

    public SyncPlayerTransform syncPlayerTransform;

    public ObjectDrag objectDrag;

    public Camera avatarCamera;

    public GameObject hand;

    float zPosition;
    Vector3 offset;
    Vector3 cursorPosition;
    public float speed;

    // Use this for initialization
    void Start () {
        zPosition = hand.transform.position.z;
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 handScreenPoint = avatarCamera.WorldToScreenPoint(hand.transform.position);

        cursorPosition = handScreenPoint;
        
    }


    // Update is called once per frame
    void Update () {
        if (isLocalPlayer) {
            if (objectDrag.controllerOn) {
                /*Vector3 handScreenPoint = avatarCamera.WorldToScreenPoint(transform.position);

                cursorPosition = handScreenPoint;*/

                cursorPosition.x = cursorPosition.x + Input.GetAxis("HorizontalDpad") * Time.deltaTime * speed;

                cursorPosition.y = cursorPosition.y + Input.GetAxis("VerticalDpad") * Time.deltaTime * speed;
                cursorPosition.z = 0.666f;

                //offset = hand.transform.position - avatarCamera.ScreenToWorldPoint(cursorPosition);
                hand.transform.position = avatarCamera.ScreenToWorldPoint(cursorPosition);

                syncPlayerTransform.UpdateHandPosition(hand.transform.position);
            } else {
                cursorPosition = Input.mousePosition;
                cursorPosition.z = 0.666f;
                //offset = hand.transform.position - avatarCamera.ScreenToWorldPoint(cursorPosition);
                hand.transform.position = avatarCamera.ScreenToWorldPoint(cursorPosition);

                syncPlayerTransform.UpdateHandPosition(hand.transform.position);
            }
        } else {

            hand.transform.position = syncPlayerTransform.getHandPosition();
        }
        
        

    }
}
