using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ManagedPhantom;
using CHSF;


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

    // Script associated to the animated hand's mesh, that closes or opens this hand
    HandLerp handLerp;

    [SerializeField]
    HandCollider handColliderScript;

    [SerializeField]
    Camera avatarCamera;

    GameObject objectToSelect;

    SoundManager soundManager;

    // Boolean to know if we are in front of the patient when we are the surgeon
	bool isInFrontOfPatient;
    

    // The int value of the layer mask "Selectable"
    int layerSelectable;

    // The angles for spherical rotation of the hand around the player, using controller
    float horizontalAngle;
    float verticalAngle;

    [SerializeField]
    [Tooltip("The sensitivity of the controller")]
    float speed;

    // Haptic manager
    public HapticManager hapticManager;

    // The config for the local instance
    ConfigInitializer config;

    SkinnedMeshRenderer handMesh;

    public GameObject ObjectToSelect {
        set {
            objectToSelect = value;
        }
    }


    /// <summary>
    ///     Update both the position and rotation of the avatar's hand in the local instance and on the network
    /// </summary>
    public void SetHandTransform(Vector3 newPosition, Quaternion newRotation) {
        hand.transform.position = newPosition;
        hand.transform.rotation = newRotation;
    }

    /// <summary>
    ///     Close the hand mesh
    /// </summary>
    public void CloseHand() {
        handLerp.Play();
    }

    /// <summary>
    ///     Open the hand mesh
    /// </summary>
    public void OpenHand() {
        handLerp.Revert();
    }

    /// <summary>
    ///     Set the hand mesh's state (active or inactive). Must be called if a laser is used for instance.
    /// </summary>
    public void SetHandMeshActive(bool isActive) {
        handMesh.enabled = isActive;
    }


    void Start() {
        horizontalAngle = 0f;
        verticalAngle = 0f;

        objectToSelect = null;

        config = GameObject.FindObjectOfType<ConfigInitializer>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();

        isInFrontOfPatient = true;

        handMesh = hand.GetComponentInChildren<SkinnedMeshRenderer>();
        handLerp = hand.GetComponent<HandLerp>();
    }


    void Update() {
        if (isLocalPlayer) {
            if (config.GetInputDevice() != InputDevice.Remote) {
                // Using controller
                if (config.GetInputDevice() == InputDevice.Controller) {
                    Vector3 newpos = hand.transform.position;

                    horizontalAngle = horizontalAngle + Input.GetAxis("HorizontalDpad") * Time.deltaTime * speed;
                    verticalAngle = verticalAngle + Input.GetAxis("VerticalDpad") * Time.deltaTime * speed;
                    verticalAngle = Mathf.Clamp(verticalAngle, -1, 1);

                    newpos.z = prefabTransform.position.z - Mathf.Sin(horizontalAngle) * 0.66f;
                    newpos.x = prefabTransform.position.x + Mathf.Cos(horizontalAngle) * 0.66f + Mathf.Cos(verticalAngle) * 0.66f - 0.66f;
                    newpos.y = prefabTransform.position.y + Mathf.Sin(verticalAngle) * 0.66f;

                    //TODO WARNING, use the value angleHorizontal to move verticaly to fix a bug

                    hand.transform.position = newpos;

                    if (Input.GetButtonDown("Fire1")) {
                        if (objectDrag.GetIsDragFeatureOn()) {
                            objectDrag.ReleaseObject();
                        } else {
                            // Playing the selection sound effect
                            soundManager.PlaySelectionSound(hand.transform.position);

                            // ... start dragging the object
                            objectDrag.SelectObject(hand, objectToSelect, 0f);
                        }
                    }
                // Using haptic arm
                } else {
                    // Move the GameObject according to the haptic arm
                    hand.transform.localPosition = hapticManager.HandPosition;

                    // Rotate the GameObject according to the haptic arm
                    hand.transform.localRotation = hapticManager.HandRotation;


                    // Test if the button1 of the haptic controller is clicked
                    if (hapticManager.GetButtonDown(1)) {
                        // If an object is currently beeing draged ...
                        if (objectDrag.GetIsDragFeatureOn()) {
                            if (!handColliderScript.GetIsContactTable() && !handColliderScript.GetIsContactTissue()) {
                                //... release the object
                                objectDrag.ReleaseObject();

                                // Playing the selection sound effect
                                soundManager.PlayDropSound(hand.transform.position);

                                // Reactivate the hand and tell the haptic manager that a syringe is not selected
                                hapticManager.ReleaseSyringe();
                                SetHandMeshActive(true);
                            }
                        //If an object can be selected ...
                        } else {
                            if (objectToSelect != null) {

                                // Playing the selection sound effect
                                soundManager.PlaySelectionSound(hand.transform.position);

                                // ... start dragging the object
                                objectDrag.SelectObject(hand, objectToSelect, 0f);

                                // When the object selected is a syringe, make the hand disappear and tell the haptic manager that a syringe is selected
                                if (objectToSelect.CompareTag("Syringe")) {
                                    hapticManager.SelectSyringe();
                                    SetHandMeshActive(false);
                                }
                            }
                        }
                    }

                    // Test if the button2 of the haptic controller is clicked
                    if (hapticManager.GetButtonDown(2) && (config.GetPlayerRole() == PlayerRole.Surgeon)) {
                        if (!handColliderScript.GetIsContactTable() && !handColliderScript.GetIsContactTissue()) { 
                            if (isInFrontOfPatient) {
                                transform.Rotate(0, 60, 0);
                                isInFrontOfPatient = false;
                            } else {
                                transform.Rotate(0, -60, 0);
                                isInFrontOfPatient = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
