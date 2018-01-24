﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Hand : NetworkBehaviour {

    public SyncPlayerTransform syncPlayerTransform;

    public ObjectDrag objectDrag;

    public Transform prefabTransform;

    public GameObject hand;

    public float angleHorizontal;
    private float angleVertical;

    
    Vector3 offset;
   
    public float speed;

    // Use this for initialization
    void Start () {
        
        //Cursor.lockState = CursorLockMode.Locked;
        angleHorizontal = 0f;
        angleVertical = 0f;
        

    }


    // Update is called once per frame
    void Update () {
        if (isLocalPlayer) {
            if (objectDrag.controllerOn) {

                
                Vector3 newpos = hand.transform.position;
                
                angleHorizontal = angleHorizontal + Input.GetAxis("HorizontalDpad") * Time.deltaTime * speed;
                angleVertical = angleVertical + Input.GetAxis("VerticalDpad") * Time.deltaTime * speed;
                angleVertical = Mathf.Clamp(angleVertical, -1, 1);

                newpos.z = prefabTransform.position.z - Mathf.Sin(angleHorizontal) * 0.66f;
                newpos.x = prefabTransform.position.x + Mathf.Cos(angleHorizontal) * 0.66f + Mathf.Cos(angleVertical) * 0.66f - 0.66f;
                newpos.y = prefabTransform.position.y + Mathf.Sin(angleVertical) * 0.66f;

                //TODO WARNING, use the value angleHorizontal to move verticaly to fix a bug

                hand.transform.position = newpos;
                syncPlayerTransform.UpdateHandPosition(hand.transform.position);


            } else {
                //TODO Souris abandonné place à l'haptic
                /*
                cursorPosition = Input.mousePosition;
                cursorPosition.z = 0.666f;
                //offset = hand.transform.position - avatarCamera.ScreenToWorldPoint(cursorPosition);
                hand.transform.position = avatarCamera.ScreenToWorldPoint(cursorPosition);

                syncPlayerTransform.UpdateHandPosition(hand.transform.position);
                */
            }
        } else {

            hand.transform.position = syncPlayerTransform.getHandPosition();
        }
        
        

    }
}