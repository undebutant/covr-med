using System.Collections;
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
    float horizontalAngle;
    float verticalAngle;

    [SerializeField]
    [Tooltip("The sensitivity of the controller")]
    float speed;

    //Haptic manager
    public HapticManager hapticManager;

    // The config for the local instance
    ConfigInitializer config;


    /// <summary>
    ///     Update both the position and rotation of the avatar's hand in the local instance and on the network
    /// </summary>
    public void SetHandTransform(Vector3 newPosition, Quaternion newRotation) {
        hand.transform.position = newPosition;
        hand.transform.rotation = newRotation;

        syncPlayerTransform.UpdateHandPosition(hand.transform.position);
    }


    void Start() {
        horizontalAngle = 0f;
        verticalAngle = 0f;

        layerSelectable = LayerMask.NameToLayer("selectionable");

        config = GameObject.FindObjectOfType<ConfigInitializer>();
    }


    void Update() {
        if (isLocalPlayer) {
            if (config.GetInputDevice() != InputDevice.Remote) {
                // Using controller
                if (inputManager.controllerOn) {
                    Vector3 newpos = hand.transform.position;

                    horizontalAngle = horizontalAngle + Input.GetAxis("HorizontalDpad") * Time.deltaTime * speed;
                    verticalAngle = verticalAngle + Input.GetAxis("VerticalDpad") * Time.deltaTime * speed;
                    verticalAngle = Mathf.Clamp(verticalAngle, -1, 1);

                    newpos.z = prefabTransform.position.z - Mathf.Sin(horizontalAngle) * 0.66f;
                    newpos.x = prefabTransform.position.x + Mathf.Cos(horizontalAngle) * 0.66f + Mathf.Cos(verticalAngle) * 0.66f - 0.66f;
                    newpos.y = prefabTransform.position.y + Mathf.Sin(verticalAngle) * 0.66f;

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
                // Using haptic arm
                } else {
                    // Move the GameObject according to the haptic arm
                    hand.transform.localPosition = hapticManager.HandPosition;
                    // Rotate the GameObject according to the haptic arm
                    hand.transform.localRotation = hapticManager.HandRotation;

                    syncPlayerTransform.UpdateHandPosition(hand.transform.position);
                }
            }
        // If it is not a local player, only update the hand
        } else {
            hand.transform.position = syncPlayerTransform.getHandPosition();
        }
    }
}
