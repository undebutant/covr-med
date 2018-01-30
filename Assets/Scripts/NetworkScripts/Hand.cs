﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Hand : NetworkBehaviour {

    [SerializeField]
    [Tooltip("The network script for online synchronisation of the player")]
    SyncPlayerTransform syncPlayerTransform;

    [SerializeField]
    [Tooltip("The drag and drop script coming from the hand")]
    ObjectDrag objectDrag;

    [SerializeField]
    [Tooltip("The input manager of the player")]
    InputManager inputManager;

    [SerializeField]
    [Tooltip("The transform of the parent avatar")]
    Transform prefabTransform;

    [SerializeField]
    [Tooltip("The hand GameObject of this avatar")]
    GameObject hand;

    [SerializeField]
    Camera avatarCamera;

    // The int value of the layer mask "selectionable"
    int layerSelectable;

    // The angles for spherical rotation of the hand around the player, using controller
    float angleHorizontal;
    float angleVertical;

    [SerializeField]
    [Tooltip("The sensitivity of the controller")]
    float speed;

    //Haptic manager
    public HapticManager hapticManager;

    ConfigInitializer config;

    public void SetHandTransform(Vector3 newPosition, Quaternion newRotation){
        Debug.LogError("Nouvelle position pour la main : " + newPosition);
        hand.transform.position = newPosition;
        hand.transform.rotation = newRotation;
        syncPlayerTransform.UpdateHandPosition(hand.transform.position);
    }

    void Start () {
        angleHorizontal = 0f;
        angleVertical = 0f;

        layerSelectable = LayerMask.NameToLayer("selectionable");

        config = GameObject.FindObjectOfType<ConfigInitializer>();
    }


    void Update () {
        Debug.LogError(config.GetInputDevice());
        if (isLocalPlayer) {
            syncPlayerTransform.UpdateHandPosition(hand.transform.position);
            if (config.GetInputDevice() != InputDevice.Remote) {
                // Using controller
                if (inputManager.controllerOn) {
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

                    if (Input.GetButtonDown("Fire1")) {
                        if (objectDrag.GetIsDragFeatureOn()) {
                            objectDrag.ReleaseObject();
                        } else {
                            // Raycast for the controller only
                            Ray ray = avatarCamera.ScreenPointToRay(avatarCamera.WorldToScreenPoint(hand.transform.position));

                            RaycastHit shootHit;

                            // Whenever the rayCast hits something...
                            if (Physics.Raycast(ray, out shootHit, 1000)) {
                                // ... matching the layer
                                if (shootHit.collider.gameObject.layer == layerSelectable) {
                                    objectDrag.SelectObject(hand, shootHit.collider.gameObject, 0f);
                                }
                            }
                        }
                    }


                } else {

                    // Move the GameObject according to the haptic arm
                    hand.transform.localPosition = hapticManager.HandPosition;
                    // Rotate the GameObject according to the haptic arm
                    hand.transform.localRotation = hapticManager.HandRotation;

                    syncPlayerTransform.UpdateHandPosition(hand.transform.position);


                }
            } else {
                hand.transform.position = syncPlayerTransform.getHandPosition();
            }
        }
    }
}
